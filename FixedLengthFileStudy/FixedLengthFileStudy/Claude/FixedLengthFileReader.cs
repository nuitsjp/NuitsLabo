using System.Buffers;
using System.Text;

namespace FixedLengthFileStudy.Claude;

public class FixedLengthFileReader : IFixedLengthFileReader
{
    private readonly Stream _reader;
    private readonly Encoding _encoding;
    private readonly byte[] _newLineBytes;
    private readonly int _bufferSize;
    private byte[] _buffer;
    private int _bufferPosition;
    private int _bytesInBuffer;
    private bool _isDisposed;

    // 現在の行のデータを保持
    private byte[]? _currentLine;
    private int _currentLineLength;

    public FixedLengthFileReader(Stream reader, Encoding encoding, string newLine, int bufferSize = 4096)
    {
        _reader = reader;
        _encoding = encoding;
        _newLineBytes = encoding.GetBytes(newLine);
        _bufferSize = bufferSize;
        _buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        _bufferPosition = 0;
        _bytesInBuffer = 0;
    }

    public bool Read()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(FixedLengthFileReader));

        // 前回の行データをクリア
        if (_currentLine != null)
        {
            ArrayPool<byte>.Shared.Return(_currentLine);
            _currentLine = null;
        }

        // バッファーが空の場合は新しいデータを読み込む
        if (_bufferPosition >= _bytesInBuffer)
        {
            _bytesInBuffer = _reader.Read(_buffer, 0, _bufferSize);
            _bufferPosition = 0;

            if (_bytesInBuffer == 0)
                return false;
        }

        // 行の終わりを探す
        int lineStart = _bufferPosition;
        int lineLength = 0;
        bool foundNewLine = false;

        while (!foundNewLine)
        {
            if (_bufferPosition >= _bytesInBuffer)
            {
                // バッファーの残りをテンポラリバッファーにコピー
                byte[] tempBuffer = ArrayPool<byte>.Shared.Rent(_bufferSize);
                Buffer.BlockCopy(_buffer, lineStart, tempBuffer, 0, _bytesInBuffer - lineStart);
                lineLength = _bytesInBuffer - lineStart;

                // 新しいデータを読み込む
                _bytesInBuffer = _reader.Read(_buffer, 0, _bufferSize);
                _bufferPosition = 0;
                lineStart = 0;

                if (_bytesInBuffer == 0)
                {
                    if (lineLength > 0)
                    {
                        // 最後の行を処理
                        _currentLine = tempBuffer;
                        _currentLineLength = lineLength;
                        return true;
                    }
                    ArrayPool<byte>.Shared.Return(tempBuffer);
                    return false;
                }
            }

            // 改行文字を探す
            for (int i = 0; i < _newLineBytes.Length && _bufferPosition < _bytesInBuffer; i++)
            {
                if (_buffer[_bufferPosition] != _newLineBytes[i])
                    break;

                if (i == _newLineBytes.Length - 1)
                {
                    foundNewLine = true;
                    _bufferPosition++;
                    break;
                }
                _bufferPosition++;
            }

            if (!foundNewLine)
                _bufferPosition++;
        }

        // 行データを保存
        lineLength = _bufferPosition - lineStart - _newLineBytes.Length;
        _currentLine = ArrayPool<byte>.Shared.Rent(lineLength);
        Buffer.BlockCopy(_buffer, lineStart, _currentLine, 0, lineLength);
        _currentLineLength = lineLength;

        return true;
    }

    public string GetField(int index, int bytes)
    {
        if (_currentLine == null)
            throw new InvalidOperationException("No current record.");

        if (index < 0 || bytes <= 0 || index + bytes > _currentLineLength)
            throw new ArgumentOutOfRangeException($"Invalid field parameters: index={index}, bytes={bytes}");

        // 必要な部分だけをデコード
        return _encoding.GetString(_currentLine, index, bytes).TrimEnd();
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            if (_buffer != null)
                ArrayPool<byte>.Shared.Return(_buffer);
            if (_currentLine != null)
                ArrayPool<byte>.Shared.Return(_currentLine);
            _reader.Dispose();
            _isDisposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_isDisposed)
        {
            if (_buffer != null)
                ArrayPool<byte>.Shared.Return(_buffer);
            if (_currentLine != null)
                ArrayPool<byte>.Shared.Return(_currentLine);
            await _reader.DisposeAsync();
            _isDisposed = true;
        }
    }
}