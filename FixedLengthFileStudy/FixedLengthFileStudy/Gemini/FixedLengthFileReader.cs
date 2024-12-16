using System.Buffers;
using System.Text;

namespace FixedLengthFileStudy.Gemini;

public class FixedLengthFileReader : IFixedLengthFileReader
{
    private readonly Stream _reader;
    private readonly Decoder _decoder;
    private readonly byte[] _buffer;
    private readonly byte[] _newLineBytes;
    private int _bufferPosition;
    private int _bufferLength;
    private byte[]? _currentLineBuffer;

    public FixedLengthFileReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _decoder = encoding.GetDecoder();
        _buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        _newLineBytes = encoding.GetBytes(newLine);
    }

    public bool Read()
    {
        _bufferPosition = 0;

        // 前の行のバッファをクリア
        if (_currentLineBuffer != null)
        {
            ArrayPool<byte>.Shared.Return(_currentLineBuffer);
            _currentLineBuffer = null;
        }

        List<byte> lineBytes = new List<byte>(); // 行データを蓄積するリスト
        int newLineIndex = -1;

        while (true)
        {
            if (_bufferPosition >= _bufferLength)
            {
                _bufferLength = _reader.Read(_buffer, 0, _buffer.Length);
                _bufferPosition = 0;
                if (_bufferLength == 0 && lineBytes.Count == 0) return false; // EOFかつ行データなし
                if (_bufferLength == 0) break; //EOFだが行データはある場合はループを抜ける
            }

            for (int i = _bufferPosition; i < _bufferLength; i++)
            {
                if (CheckNewLine(_buffer, i))
                {
                    newLineIndex = i;
                    break;
                }
            }
            int readLength = (newLineIndex == -1) ? _bufferLength - _bufferPosition : newLineIndex - _bufferPosition;

            //行データを蓄積
            lineBytes.AddRange(_buffer.AsSpan(_bufferPosition, readLength).ToArray());
            _bufferPosition += readLength + (newLineIndex > -1 ? _newLineBytes.Length : 0);

            if (newLineIndex > -1) break;
        }


        _currentLineBuffer = ArrayPool<byte>.Shared.Rent(lineBytes.Count);
        lineBytes.ToArray().CopyTo(_currentLineBuffer.AsSpan());
        _bufferPosition = 0;
        _bufferLength = lineBytes.Count;

        return true;
    }

    private bool CheckNewLine(byte[] buffer, int index)
    {
        if (index + _newLineBytes.Length > buffer.Length) return false;

        for (int i = 0; i < _newLineBytes.Length; i++)
        {
            if (buffer[index + i] != _newLineBytes[i]) return false;
        }
        return true;
    }

    public ReadOnlySpan<byte> GetFieldBytes(int index, int bytes)
    {
        if (_currentLineBuffer == null)
        {
            throw new InvalidOperationException("Read method must be called first.");
        }

        if (index < 0 || index + bytes > _bufferLength)
        {
            throw new IndexOutOfRangeException();
        }

        return new ReadOnlySpan<byte>(_currentLineBuffer, index, bytes);
    }

    public string GetField(int index, int bytes)
    {
        var byteSpan = GetFieldBytes(index, bytes);

        int maxCharCount = Encoding.UTF8.GetMaxCharCount(byteSpan.Length);
        char[] chars = ArrayPool<char>.Shared.Rent(maxCharCount);
        int actualCharCount = _decoder.GetChars(byteSpan, chars, true);
        string result = new string(chars, 0, actualCharCount);
        ArrayPool<char>.Shared.Return(chars);
        return result;
    }

    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        if (_currentLineBuffer != null) ArrayPool<byte>.Shared.Return(_currentLineBuffer);
        _reader.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        if (_currentLineBuffer != null) ArrayPool<byte>.Shared.Return(_currentLineBuffer);
        await _reader.DisposeAsync();
    }
}