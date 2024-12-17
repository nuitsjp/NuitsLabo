// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FixedLengthFileStudy;

// This class implements a TextReader for reading characters to a Stream.
// This is designed for character input in a particular Encoding,
// whereas the Stream class is designed for byte input and output.
public class ByteStreamReader : IDisposable, IAsyncDisposable
{
    // Using a 1K byte buffer and a 4K FileStream buffer works out pretty well
    // perf-wise.  On even a 40 MB text file, any perf loss by using a 4K
    // buffer is negated by the win of allocating a smaller byte[], which
    // saves construction time.  This does break adaptive buffering,
    // but this is slightly faster.
    public static readonly int DefaultBufferSize = 4096;  // Byte buffer size
    public static readonly int MinBufferSize = 128;

    private readonly Stream _stream;
    private readonly byte[] _byteBuffer = null!; // only null in NullStreamReader where this is never used
    //private char[] _charBuffer = null!; // only null in NullStreamReader where this is never used
    private int _charPos;
    // Record the number of valid bytes in the byteBuffer, for a few checks.
    private int _byteLen;
    // This is used only for preamble detection
    private int _bytePos;

    /// <summary>True if the writer has been disposed; otherwise, false.</summary>
    private bool _disposed;

    // We don't guarantee thread safety on ByteStreamReader, but we should at
    // least prevent users from trying to read anything while an Async
    // read from the same thread is in progress.
    private Task _asyncReadTask = Task.CompletedTask;

    private void CheckAsyncTaskInProgress()
    {
        // We are not locking the access to _asyncReadTask because this is not meant to guarantee thread safety.
        // We are simply trying to deter calling any Read APIs while an async Read from the same thread is in progress.
        if (!_asyncReadTask.IsCompleted)
        {
            throw new InvalidOperationException();
        }
    }

    public ByteStreamReader(Stream stream, int? bufferSize = null)
    {
        int byteBufferSize;
        if (bufferSize is null)
        {
            byteBufferSize = DefaultBufferSize;
        }
        else if (bufferSize < MinBufferSize)
        {
            throw new ArgumentOutOfRangeException(nameof(bufferSize));
        }
        else
        {
            byteBufferSize = bufferSize.Value;
        }

        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream can not read.");
        }

        _stream = stream;

        _byteBuffer = new byte[byteBufferSize];
    }

    public void Close()
    {
        Dispose(true);
    }


    protected void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        try
        {
            // Note that Stream.Close() can potentially throw here. So we need to
            // ensure cleaning up internal resources, inside the finally block.
            if (disposing)
            {
                _stream.Close();
            }
        }
        finally
        {
            _charPos = 0;
            _byteLen = 0;
        }
    }

    //internal virtual int ReadBuffer()
    //{
    //    _charLen = 0;
    //    _charPos = 0;
    //    _byteLen = 0;

    //    var eofReached = false;

    //    do
    //    {
    //        Debug.Assert(_bytePos == 0, "bytePos can be non zero only when we are trying to _checkPreamble.  Are two threads using this ByteStreamReader at the same time?");
    //        _byteLen = _stream.Read(_byteBuffer, 0, _byteBuffer.Length);
    //        Debug.Assert(_byteLen >= 0, "Stream.Read returned a negative number!  This is a bug in your stream class.");

    //        if (_byteLen == 0)
    //        {
    //            eofReached = true;
    //            break;
    //        }

    //        Debug.Assert(_charPos == 0 && _charLen == 0, "We shouldn't be trying to decode more data if we made progress in an earlier iteration.");
    //        _charLen = _decoder.GetChars(_byteBuffer, 0, _byteLen, _charBuffer, 0, flush: false);
    //    } while (_charLen == 0);

    //    if (eofReached)
    //    {
    //        // EOF has been reached - perform final flush.
    //        // We need to reset _bytePos and _byteLen just in case we hadn't
    //        // finished processing the preamble before we reached EOF.

    //        Debug.Assert(_charPos == 0 && _charLen == 0, "We shouldn't be looking for EOF unless we have an empty char buffer.");
    //        _charLen = _decoder.GetChars(_byteBuffer, 0, _byteLen, _charBuffer, 0, flush: true);
    //        _bytePos = 0;
    //        _byteLen = 0;
    //    }

    //    return _charLen;
    //}

    private int ReadByteBuffer()
    {
        _charPos = 0;
        _byteLen = _stream.Read(_byteBuffer, 0, _byteBuffer.Length);
        return _byteLen;
    }

    // Reads a line. A line is defined as a sequence of characters followed by
    // a carriage return ('\r'), a line feed ('\n'), or a carriage return
    // immediately followed by a line feed. The resulting string does not
    // contain the terminating carriage return and/or line feed. The returned
    // value is null if the end of the input stream has been reached.
    //
    //public string? ReadLine()
    //{
    //    CheckAsyncTaskInProgress();

    //    if (_charPos == _charLen)
    //    {
    //        if (ReadBuffer() == 0)
    //        {
    //            return null;
    //        }
    //    }

    //    var vsb = new ValueStringBuilder(stackalloc char[256]);
    //    do
    //    {
    //        // Look for '\r' or \'n'.
    //        ReadOnlySpan<char> charBufferSpan = _charBuffer.AsSpan(_charPos, _charLen - _charPos);
    //        Debug.Assert(!charBufferSpan.IsEmpty, "ReadBuffer returned > 0 but didn't bump _charLen?");

    //        var idxOfNewline = charBufferSpan.IndexOfAny('\r', '\n');
    //        if (idxOfNewline >= 0)
    //        {
    //            string retVal;
    //            if (vsb.Length == 0)
    //            {
    //                retVal = new string(charBufferSpan.Slice(0, idxOfNewline));
    //            }
    //            else
    //            {
    //                retVal = string.Concat(vsb.AsSpan(), charBufferSpan.Slice(0, idxOfNewline));
    //                vsb.Dispose();
    //            }

    //            var matchedChar = charBufferSpan[idxOfNewline];
    //            _charPos += idxOfNewline + 1;

    //            // If we found '\r', consume any immediately following '\n'.
    //            if (matchedChar == '\r')
    //            {
    //                if (_charPos < _charLen || ReadBuffer() > 0)
    //                {
    //                    if (_charBuffer[_charPos] == '\n')
    //                    {
    //                        _charPos++;
    //                    }
    //                }
    //            }

    //            return retVal;
    //        }

    //        // We didn't find '\r' or '\n'. Add it to the StringBuilder
    //        // and loop until we reach a newline or EOF.

    //        vsb.Append(charBufferSpan);
    //    } while (ReadBuffer() > 0);

    //    return vsb.ToString();
    //}


    // Reads a line. A line is defined as a sequence of characters followed by
    // a carriage return ('\r'), a line feed ('\n'), or a carriage return
    // immediately followed by a line feed. The resulting string does not
    // contain the terminating carriage return and/or line feed. The returned
    // value is null if the end of the input stream has been reached.
    //
    public byte[]? ReadLine()
    {
        CheckAsyncTaskInProgress();

        if (_charPos == _byteLen)
        {
            if (ReadByteBuffer() == 0)
            {
                return null;
            }
        }

        var vsb = new ValueByteArrayBuilder(stackalloc byte[256]);
        do
        {
            // Look for '\r' or \'n'.
            ReadOnlySpan<byte> bufferSpan = _byteBuffer.AsSpan(_charPos, _byteLen - _charPos);
            Debug.Assert(!bufferSpan.IsEmpty, "ReadBuffer returned > 0 but didn't bump _charLen?");

            var idxOfNewline = bufferSpan.IndexOfAny((byte)'\r', (byte)'\n');
            if (idxOfNewline >= 0)
            {
                byte[] retVal;
                if (vsb.Length == 0)
                {
                    retVal = bufferSpan.Slice(0, idxOfNewline).ToArray();
                }
                else
                {
                    retVal = Concat(vsb.AsSpan(), bufferSpan.Slice(0, idxOfNewline));
                    vsb.Dispose();
                }

                var matchedChar = bufferSpan[idxOfNewline];
                _charPos += idxOfNewline + 1;

                // If we found '\r', consume any immediately following '\n'.
                if (matchedChar == '\r')
                {
                    if (_charPos < _byteLen || ReadByteBuffer() > 0)
                    {
                        if (bufferSpan[idxOfNewline + 1] == '\n')
                        {
                            _charPos++;
                        }
                    }
                }

                return retVal;
            }

            // We didn't find '\r' or '\n'. Add it to the StringBuilder
            // and loop until we reach a newline or EOF.

            vsb.Append(bufferSpan);
        } while (ReadByteBuffer() > 0);

        return vsb.AsSpan().ToArray();
    }

    //public Task<string?> ReadLineAsync() =>
    //    ReadLineAsync(default).AsTask();

    /// <summary>
    /// Reads a line of characters asynchronously from the current stream and returns the data as a string.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A value task that represents the asynchronous read operation. The value of the <c>TResult</c>
    /// parameter contains the next line from the stream, or is <see langword="null" /> if all of the characters have been read.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="int.MaxValue"/>.</exception>
    /// <exception cref="ObjectDisposedException">The stream reader has been disposed.</exception>
    /// <exception cref="InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <example>
    /// The following example shows how to read and print all lines from the file until the end of the file is reached or the operation timed out.
    /// <code lang="C#">
    /// using CancellationTokenSource tokenSource = new (TimeSpan.FromSeconds(1));
    /// using ByteStreamReader reader = File.OpenText("existingfile.txt");
    ///
    /// string line;
    /// while ((line = await reader.ReadLineAsync(tokenSource.Token)) is not null)
    /// {
    ///     Console.WriteLine(line);
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// If this method is canceled via <paramref name="cancellationToken"/>, some data
    /// that has been read from the current <see cref="Stream"/> but not stored (by the
    /// <see cref="ByteStreamReader"/>) or returned (to the caller) may be lost.
    /// </remarks>
    //public ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken)
    //{
    //    CheckAsyncTaskInProgress();

    //    Task<string?> task = ReadLineAsyncInternal(cancellationToken);
    //    _asyncReadTask = task;

    //    return new ValueTask<string?>(task);
    //}

    //private async Task<string?> ReadLineAsyncInternal(CancellationToken cancellationToken)
    //{
    //    if (_charPos == _charLen && (await ReadBufferAsync(cancellationToken).ConfigureAwait(false)) == 0)
    //    {
    //        return null;
    //    }

    //    string retVal;
    //    char[]? arrayPoolBuffer = null;
    //    var arrayPoolBufferPos = 0;

    //    do
    //    {
    //        var charBuffer = _charBuffer;
    //        var charLen = _charLen;
    //        var charPos = _charPos;

    //        // Look for '\r' or \'n'.
    //        Debug.Assert(charPos < charLen, "ReadBuffer returned > 0 but didn't bump _charLen?");

    //        var idxOfNewline = charBuffer.AsSpan(charPos, charLen - charPos).IndexOfAny('\r', '\n');
    //        if (idxOfNewline >= 0)
    //        {
    //            if (arrayPoolBuffer is null)
    //            {
    //                retVal = new string(charBuffer, charPos, idxOfNewline);
    //            }
    //            else
    //            {
    //                retVal = string.Concat(arrayPoolBuffer.AsSpan(0, arrayPoolBufferPos), charBuffer.AsSpan(charPos, idxOfNewline));
    //                ArrayPool<char>.Shared.Return(arrayPoolBuffer);
    //            }

    //            charPos += idxOfNewline;
    //            var matchedChar = charBuffer[charPos++];
    //            _charPos = charPos;

    //            // If we found '\r', consume any immediately following '\n'.
    //            if (matchedChar == '\r')
    //            {
    //                if (charPos < charLen || (await ReadBufferAsync(cancellationToken).ConfigureAwait(false)) > 0)
    //                {
    //                    if (_charBuffer[_charPos] == '\n')
    //                    {
    //                        _charPos++;
    //                    }
    //                }
    //            }

    //            return retVal;
    //        }

    //        // We didn't find '\r' or '\n'. Add the read data to the pooled buffer
    //        // and loop until we reach a newline or EOF.
    //        if (arrayPoolBuffer is null)
    //        {
    //            arrayPoolBuffer = ArrayPool<char>.Shared.Rent(charLen - charPos + 80);
    //        }
    //        else if ((arrayPoolBuffer.Length - arrayPoolBufferPos) < (charLen - charPos))
    //        {
    //            var newBuffer = ArrayPool<char>.Shared.Rent(checked(arrayPoolBufferPos + charLen - charPos));
    //            arrayPoolBuffer.AsSpan(0, arrayPoolBufferPos).CopyTo(newBuffer);
    //            ArrayPool<char>.Shared.Return(arrayPoolBuffer);
    //            arrayPoolBuffer = newBuffer;
    //        }
    //        charBuffer.AsSpan(charPos, charLen - charPos).CopyTo(arrayPoolBuffer.AsSpan(arrayPoolBufferPos));
    //        arrayPoolBufferPos += charLen - charPos;
    //    }
    //    while (await ReadBufferAsync(cancellationToken).ConfigureAwait(false) > 0);

    //    if (arrayPoolBuffer is not null)
    //    {
    //        retVal = new string(arrayPoolBuffer, 0, arrayPoolBufferPos);
    //        ArrayPool<char>.Shared.Return(arrayPoolBuffer);
    //    }
    //    else
    //    {
    //        retVal = string.Empty;
    //    }

    //    return retVal;
    //}

    //private async ValueTask<int> ReadBufferAsync(CancellationToken cancellationToken)
    //{
    //    _charLen = 0;
    //    _charPos = 0;
    //    var tmpByteBuffer = _byteBuffer;
    //    var tmpStream = _stream;
    //    _byteLen = 0;

    //    var eofReached = false;

    //    do
    //    {
    //        Debug.Assert(_bytePos == 0, "_bytePos can be non zero only when we are trying to _checkPreamble. Are two threads using this ByteStreamReader at the same time?");
    //        _byteLen = await tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer), cancellationToken).ConfigureAwait(false);
    //        Debug.Assert(_byteLen >= 0, "Stream.Read returned a negative number!  Bug in stream class.");

    //        if (_byteLen == 0)
    //        {
    //            eofReached = true;
    //            break;
    //        }

    //        Debug.Assert(_charPos == 0 && _charLen == 0, "We shouldn't be trying to decode more data if we made progress in an earlier iteration.");
    //        _charLen = _decoder.GetChars(tmpByteBuffer, 0, _byteLen, _charBuffer, 0, flush: false);
    //    } while (_charLen == 0);

    //    if (eofReached)
    //    {
    //        // EOF has been reached - perform final flush.
    //        // We need to reset _bytePos and _byteLen just in case we hadn't
    //        // finished processing the preamble before we reached EOF.

    //        Debug.Assert(_charPos == 0 && _charLen == 0, "We shouldn't be looking for EOF unless we have an empty char buffer.");
    //        _charLen = _decoder.GetChars(_byteBuffer, 0, _byteLen, _charBuffer, 0, flush: true);
    //        _bytePos = 0;
    //        _byteLen = 0;
    //    }

    //    return _charLen;
    //}

    public void Dispose()
    {
        Dispose(true);
    }

    public async ValueTask DisposeAsync()
    {
        Dispose(true);
    }

    private static byte[] Concat(ReadOnlySpan<byte> span1, ReadOnlySpan<byte> span2)
    {
        // 新しい配列を作成
        var combined = new byte[span1.Length + span2.Length];

        // 両方のSpanをコピー
        span1.CopyTo(combined.AsSpan(0));
        span2.CopyTo(combined.AsSpan(span1.Length));

        return combined;
    }
}