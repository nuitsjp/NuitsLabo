using System.Buffers;
using System.Text;

namespace FixedLengthFileStudy.Gemini;

public class FixedLengthFileReader : IDisposable, IAsyncDisposable
{
    private readonly Stream _reader;
    private readonly Decoder _decoder;
    private readonly byte[] _buffer;
    private readonly byte[] _newLineBytes;
    private int _bufferPosition;
    private int _bufferLength;

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

        int newLineIndex = -1;
        while (true)
        {
            if (_bufferPosition >= _bufferLength)
            {
                _bufferLength = _reader.Read(_buffer, 0, _buffer.Length);
                _bufferPosition = 0;
                if (_bufferLength == 0) return false; // EOF
            }

            for (int i = _bufferPosition; i < _bufferLength; i++)
            {
                if (CheckNewLine(_buffer, i))
                {
                    newLineIndex = i + _newLineBytes.Length;
                    break;
                }
            }
            if (newLineIndex > -1) break;
        }

        _bufferPosition = newLineIndex;
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

    public ReadOnlySpan<byte> GetField(int index, int bytes)
    {
        if (index + bytes > _bufferPosition)
        {
            throw new IndexOutOfRangeException();
        }
        return new ReadOnlySpan<byte>(_buffer, index, bytes);
    }

    public string GetFieldString(int index, int bytes)
    {
        var byteSpan = GetField(index, bytes);

        // 最大文字数を取得
        int maxCharCount = Encoding.UTF8.GetMaxCharCount(bytes);
        char[] chars = ArrayPool<char>.Shared.Rent(maxCharCount);

        // GetCharsの正しい呼び出し方: flushパラメータを渡す
        int actualCharCount = _decoder.GetChars(byteSpan, chars, true); // flushをtrueに設定

        string result = new string(chars, 0, actualCharCount);
        ArrayPool<char>.Shared.Return(chars);
        return result;
    }

    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        _reader.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        ArrayPool<byte>.Shared.Return(_buffer);
        await _reader.DisposeAsync();
    }
}