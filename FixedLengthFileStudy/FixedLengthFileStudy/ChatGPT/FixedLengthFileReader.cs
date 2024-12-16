using System.Text;

namespace FixedLengthFileStudy.ChatGPT;

public class FixedLengthFileReader : IFixedLengthFileReader
{
    private readonly Stream _reader;
    private readonly Encoding _encoding;
    private readonly byte[] _newlineBytes;
    private readonly int _bufferSize;
    private readonly byte[] _buffer;
    private int _bufferLength;
    private int _bufferPosition;

    public FixedLengthFileReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096)
    {
        _reader = reader;
        _encoding = encoding;
        _newlineBytes = encoding.GetBytes(newLine);
        _bufferSize = bufferSize;
        _buffer = new byte[bufferSize];
        _bufferLength = 0;
        _bufferPosition = 0;
    }

    public bool Read()
    {
        if (_bufferPosition >= _bufferLength && !FillBuffer())
        {
            return false; // End of stream
        }

        // Detect newline and skip it
        var newlineIndex = FindNewline();
        if (newlineIndex >= 0)
        {
            _bufferPosition = newlineIndex + _newlineBytes.Length;
        }
        return true;
    }

    public string GetField(int index, int bytes)
    {
        if (_bufferPosition + index + bytes > _bufferLength && !FillBuffer())
        {
            throw new InvalidOperationException("Cannot read beyond end of stream.");
        }

        ReadOnlySpan<byte> fieldBytes = new ReadOnlySpan<byte>(_buffer, _bufferPosition + index, bytes);
        return _encoding.GetString(fieldBytes);
    }

    private bool FillBuffer()
    {
        if (_bufferPosition < _bufferLength)
        {
            // Shift remaining data to the start of the buffer
            Array.Copy(_buffer, _bufferPosition, _buffer, 0, _bufferLength - _bufferPosition);
            _bufferLength -= _bufferPosition;
        }
        else
        {
            _bufferLength = 0;
        }

        _bufferPosition = 0;

        int bytesRead = _reader.Read(_buffer, _bufferLength, _bufferSize - _bufferLength);
        _bufferLength += bytesRead;

        return bytesRead > 0;
    }

    private int FindNewline()
    {
        for (int i = _bufferPosition; i <= _bufferLength - _newlineBytes.Length; i++)
        {
            if (_buffer.AsSpan(i, _newlineBytes.Length).SequenceEqual(_newlineBytes))
            {
                return i;
            }
        }

        return -1; // Newline not found
    }

    public void Dispose()
    {
        _reader.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _reader.DisposeAsync();
    }
}
