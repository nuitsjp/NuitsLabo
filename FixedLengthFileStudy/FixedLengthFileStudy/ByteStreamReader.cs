using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace FixedLengthFileStudy;

public class ByteStreamReader : IDisposable, IAsyncDisposable
{
    public static readonly int DefaultBufferSize = 4096;  // Byte buffer size
    public static readonly int MinBufferSize = 128;

    private readonly Stream _stream;
    private byte[] _byteBuffer;
    private int _byteLen;
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
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream can not read.");
        }

        if (bufferSize < MinBufferSize)
        {
            throw new ArgumentOutOfRangeException(nameof(bufferSize));
        }

        _stream = stream;
        _byteBuffer = bufferSize is null
            ? new byte[DefaultBufferSize]
            : new byte[bufferSize.Value];
    }

    // Reads a line. A line is defined as a sequence of characters followed by
    // a carriage return ('\r'), a line feed ('\n'), or a carriage return
    // immediately followed by a line feed. The resulting string does not
    // contain the terminating carriage return and/or line feed. The returned
    // value is null if the end of the input stream has been reached.
    //
    public byte[]? ReadLine()
    {
        CheckAsyncTaskInProgress();

        // If we're at the end of the buffer data, read from the stream.
        if (_bytePos == _byteLen)
        {
            // If we're already at the end of the stream, return null.
            if (ReadByteBuffer() == 0)
            {
                return null;
            }
        }

        do
        {
            // Look for '\r' or \'n'.
            ReadOnlySpan<byte> bufferSpan = _byteBuffer.AsSpan(_bytePos, _byteLen - _bytePos);
            Debug.Assert(!bufferSpan.IsEmpty, "ReadBuffer returned > 0 but didn't bump _charLen?");

            var indexOfNewline = bufferSpan.IndexOfAny((byte)'\r', (byte)'\n');
            if (0 <= indexOfNewline)
            {
                byte[] retVal = _byteBuffer.AsSpan(0, _bytePos + indexOfNewline).ToArray();

                var matchedChar = bufferSpan[indexOfNewline];
                var enterIndexOfNewline = indexOfNewline + 1;
                _bytePos += enterIndexOfNewline;

                // If we found '\r', consume any immediately following '\n'.
                if (matchedChar == '\r')
                {
                    // If we reached the end of the buffer, read the next buffer.
                    if (_bytePos == _byteLen)
                    {
                        ReadByteBuffer();
                    }

                    if (_bytePos < _byteLen)
                    {
                        if (bufferSpan[enterIndexOfNewline] == '\n')
                        {
                            _bytePos++;
                        }
                    }
                }

                var offset = _bytePos; // シフトさせたい要素数
                var span = _byteBuffer.AsSpan();
                span.Slice(offset).CopyTo(span);

                _byteLen = _byteLen - _bytePos;
                _bytePos = 0;

                return retVal;
            }

            // We didn't find '\r' or '\n'. Add it to the StringBuilder
            // and loop until we reach a newline or EOF.
            _bytePos = _byteLen;
        } while (0 < AppendReadBuffer());

        return _byteBuffer.AsSpan(0, _byteLen).ToArray();

        int ReadByteBuffer()
        {
            _bytePos = 0; // Reset the position to 0 to read from the beginning of the buffer.
            _byteLen = _stream.Read(_byteBuffer, 0, _byteBuffer.Length);
            return _byteLen;
        }

        int AppendReadBuffer()
        {

            if (_bytePos == _byteBuffer.Length)
            {
                const uint arrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

                var newCapacity = (int)Math.Min((uint)_byteBuffer.Length * 2, arrayMaxLength);
                var buffer = new byte[newCapacity];

                _byteBuffer.AsSpan().CopyTo(buffer);
                _byteBuffer = buffer;
            }
            var read = _stream.Read(_byteBuffer.AsSpan(_bytePos, _byteBuffer.Length - _bytePos));
            _byteLen += read;
            return read;
        }
    }

    public Task<byte[]?> ReadLineAsync() =>
        ReadLineAsync(default).AsTask();

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
    public ValueTask<byte[]?> ReadLineAsync(CancellationToken cancellationToken)
    {
        CheckAsyncTaskInProgress();

        Task<byte[]?> task = ReadLineAsyncInternal(cancellationToken);
        _asyncReadTask = task;

        return new ValueTask<byte[]?>(task);
    }

    private async Task<byte[]?> ReadLineAsyncInternal(CancellationToken cancellationToken)
    {
        if (_bytePos == _byteLen && (await ReadBufferAsync().ConfigureAwait(false)) == 0)
        {
            return null;
        }

        CheckAsyncTaskInProgress();

        // If we're at the end of the buffer data, read from the stream.
        if (_bytePos == _byteLen)
        {
            // If we're already at the end of the stream, return null.
            if (await ReadBufferAsync() == 0)
            {
                return null;
            }
        }

        do
        {
            // Look for '\r' or \'n'.
            ReadOnlySpan<byte> bufferSpan = _byteBuffer.AsSpan(_bytePos, _byteLen - _bytePos);
            Debug.Assert(!bufferSpan.IsEmpty, "ReadBuffer returned > 0 but didn't bump _charLen?");

            var indexOfNewline = bufferSpan.IndexOfAny((byte)'\r', (byte)'\n');
            if (0 <= indexOfNewline)
            {
                byte[] retVal = _byteBuffer.AsSpan(0, _bytePos + indexOfNewline).ToArray();

                var matchedChar = bufferSpan[indexOfNewline];
                var enterIndexOfNewline = indexOfNewline + 1;
                _bytePos += enterIndexOfNewline;

                // If we found '\r', consume any immediately following '\n'.
                if (matchedChar == '\r')
                {
                    // If we reached the end of the buffer, read the next buffer.
                    if (_bytePos == _byteLen)
                    {
                        await ReadBufferAsync();
                    }

                    if (_bytePos < _byteLen)
                    {
                        if (_byteBuffer[_bytePos] == '\n')
                        {
                            _bytePos++;
                        }
                    }
                }

                var offset = _bytePos; // シフトさせたい要素数
                var span = _byteBuffer.AsSpan();
                span.Slice(offset).CopyTo(span);

                _byteLen = _byteLen - _bytePos;
                _bytePos = 0;

                return retVal;
            }

            // We didn't find '\r' or '\n'. Add it to the StringBuilder
            // and loop until we reach a newline or EOF.
            _bytePos = _byteLen;
        } while (0 < await AppendReadBufferAsync());

        return _byteBuffer.AsSpan(0, _byteLen).ToArray();

        async Task<int> ReadBufferAsync()
        {
            _bytePos = 0; // Reset the position to 0 to read from the beginning of the buffer.
            _byteLen = await _stream.ReadAsync(_byteBuffer, 0, _byteBuffer.Length, cancellationToken);
            return _byteLen;
        }

        async Task<int> AppendReadBufferAsync()
        {

            if (_bytePos == _byteBuffer.Length)
            {
                const uint arrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

                var newCapacity = (int)Math.Min((uint)_byteBuffer.Length * 2, arrayMaxLength);
                var buffer = new byte[newCapacity];

                _byteBuffer.AsSpan().CopyTo(buffer);
                _byteBuffer = buffer;
            }
            var read = await _stream.ReadAsync(_byteBuffer,_bytePos, _byteBuffer.Length - _bytePos, cancellationToken);
            _byteLen += read;
            return read;
        }
    }

    public void Close()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            _stream.Close();
        }
        catch
        {
            // Ignore any exceptions that might result from closing the stream.
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            await _stream.DisposeAsync();
        }
        catch
        {
            // Ignore any exceptions that might result from closing the stream.
        }
    }
}