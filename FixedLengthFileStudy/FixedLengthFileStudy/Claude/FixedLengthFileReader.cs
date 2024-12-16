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
        int totalLineLength = 0;
        bool foundNewLine = false;
        List<(byte[] Buffer, int Start, int Length)> lineSegments = new();

        while (!foundNewLine)
        {
            if (_bufferPosition >= _bytesInBuffer)
            {
                // 現在のバッファーの残りを保存
                int remainingLength = _bytesInBuffer - lineStart;
                if (remainingLength > 0)
                {
                    byte[] segment = ArrayPool<byte>.Shared.Rent(remainingLength);
                    Buffer.BlockCopy(_buffer, lineStart, segment, 0, remainingLength);
                    lineSegments.Add((segment, 0, remainingLength));
                    totalLineLength += remainingLength;
                }

                // 新しいデータを読み込む
                _bytesInBuffer = _reader.Read(_buffer, 0, _bufferSize);
                _bufferPosition = 0;
                lineStart = 0;

                if (_bytesInBuffer == 0)
                {
                    if (totalLineLength > 0)
                    {
                        // 最後の行を処理
                        CombineLineSegments(lineSegments, totalLineLength);
                        return true;
                    }

                    // セグメントを解放
                    foreach (var segment in lineSegments)
                    {
                        ArrayPool<byte>.Shared.Return(segment.Buffer);
                    }
                    return false;
                }
            }

            // 改行文字を探す
            int newLineMatchCount = 0;
            while (_bufferPosition < _bytesInBuffer)
            {
                if (_buffer[_bufferPosition] == _newLineBytes[newLineMatchCount])
                {
                    newLineMatchCount++;
                    if (newLineMatchCount == _newLineBytes.Length)
                    {
                        foundNewLine = true;
                        break;
                    }
                }
                else
                {
                    _bufferPosition -= newLineMatchCount;
                    newLineMatchCount = 0;
                }
                _bufferPosition++;
            }

            if (foundNewLine)
            {
                // 現在の行セグメントを追加（改行文字を除く）
                int currentLength = _bufferPosition - lineStart - _newLineBytes.Length + 1;
                if (currentLength > 0)
                {
                    byte[] segment = ArrayPool<byte>.Shared.Rent(currentLength);
                    Buffer.BlockCopy(_buffer, lineStart, segment, 0, currentLength);
                    lineSegments.Add((segment, 0, currentLength));
                    totalLineLength += currentLength;
                }
                _bufferPosition++; // 改行文字の後ろに移動
            }
        }

        // 行セグメントを結合
        CombineLineSegments(lineSegments, totalLineLength);
        return true;
    }

    private void CombineLineSegments(List<(byte[] Buffer, int Start, int Length)> segments, int totalLength)
    {
        _currentLine = ArrayPool<byte>.Shared.Rent(totalLength);
        _currentLineLength = totalLength;

        int offset = 0;
        foreach (var segment in segments)
        {
            Buffer.BlockCopy(segment.Buffer, segment.Start, _currentLine, offset, segment.Length);
            offset += segment.Length;
            ArrayPool<byte>.Shared.Return(segment.Buffer);
        }
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