using System.Buffers;
using System.Text;

namespace FixedLengthFileStudy.Claude;

public class FixedLengthFileReader : IFixedLengthFileReader
{
    private readonly Stream _reader;
    private readonly Encoding _encoding;
    private readonly byte[] _newLineBytes;
    private int _bufferSize;
    private byte[] _buffer;
    private int _bufferPosition;
    private int _bytesInBuffer;
    private bool _isDisposed;
    private bool _isFirstLine = true;

    // 現在の行のデータを保持
    private byte[]? _currentLine;
    private int _currentLineLength;

    public FixedLengthFileReader(Stream reader, Encoding encoding, string newLine, Trim trim = Trim.StartAndEnd, int bufferSize = 4096)
    {
        _reader = reader;
        _encoding = encoding;
        Trim = trim;
        _newLineBytes = encoding.GetBytes(newLine);
        _bufferSize = bufferSize;
        _buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        _bufferPosition = 0;
        _bytesInBuffer = 0;
    }

    public Trim Trim { get; }

    public bool Read()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(FixedLengthFileReader));
        }

        // 前回の行データをクリア
        if (_currentLine != null)
        {
            ArrayPool<byte>.Shared.Return(_currentLine);
            _currentLine = null;
        }

        // バッファーが空の場合は新しいデータを読み込む
        if (_bufferPosition >= _bytesInBuffer)
        {
            _bytesInBuffer = _reader.Read(_buffer, 0, _buffer.Length);
            _bufferPosition = 0;

            if (_bytesInBuffer == 0)
                return false;
        }

        int lineStart = _bufferPosition;
        bool foundNewLine = false;
        int newLineMatchCount = 0;

        while (!foundNewLine)
        {
            // バッファー内を検索
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
                break;
            }

            // バッファーを使い切ったが改行が見つからない場合
            if (_bufferPosition >= _bytesInBuffer)
            {
                // 現在のバッファーの使用済み部分を除いた実効サイズを計算
                int effectiveSize = _bytesInBuffer - lineStart;

                // 新しいバッファーサイズを計算（現在のサイズ + 基本バッファーサイズ）
                int newSize = _buffer.Length + _bufferSize;
                byte[] newBuffer = ArrayPool<byte>.Shared.Rent(newSize);

                // 既存データを新バッファーの先頭にコピー
                if (effectiveSize > 0)
                {
                    Buffer.BlockCopy(_buffer, lineStart, newBuffer, 0, effectiveSize);
                }

                // 古いバッファーを返却
                ArrayPool<byte>.Shared.Return(_buffer);

                // 新しいバッファーを設定
                _buffer = newBuffer;

                // 追加データを読み込む
                int additionalBytes = _reader.Read(_buffer, effectiveSize, _buffer.Length - effectiveSize);
                if (additionalBytes == 0)
                {
                    if (effectiveSize > 0)
                    {
                        // 最後の行として処理
                        _currentLine = ArrayPool<byte>.Shared.Rent(effectiveSize);
                        Buffer.BlockCopy(_buffer, 0, _currentLine, 0, effectiveSize);
                        _currentLineLength = effectiveSize;
                        _bufferPosition = effectiveSize;
                        _bytesInBuffer = effectiveSize;
                        return true;
                    }
                    return false;
                }

                // バッファー状態を更新
                _bytesInBuffer = effectiveSize + additionalBytes;
                _bufferPosition = effectiveSize;
                lineStart = 0;
            }
        }

        // 行データを保存（改行文字を除く）
        int lineLength = _bufferPosition - lineStart - _newLineBytes.Length + 1;
        _currentLine = ArrayPool<byte>.Shared.Rent(lineLength);
        Buffer.BlockCopy(_buffer, lineStart, _currentLine, 0, lineLength);
        _currentLineLength = lineLength;
        _bufferPosition++; // 改行文字の後ろに移動

        return true;
    }

    public string GetField(int index, int bytes)
    {
        if (_currentLine == null)
        {
            throw new InvalidOperationException("No current record.");
        }

        if (_currentLineLength <= index)
        {
            throw new IndexOutOfRangeException();
        }
        if (_currentLineLength < index + bytes)
        {
            throw new ArgumentOutOfRangeException($"Line length: {_currentLineLength} index:{index} bytes:{bytes} index+bytes:{index + bytes}");
        }

        if (index < 0 || bytes <= 0)
            throw new ArgumentOutOfRangeException($"Invalid field parameters: index={index}, bytes={bytes}");

        // 必要な部分だけをデコード
        var field = _encoding.GetString(_currentLine, index, bytes);
        return Trim switch
        {
            Trim.StartAndEnd => field.Trim(),
            Trim.Start => field.TrimStart(),
            Trim.End => field.TrimEnd(),
            Trim.None => field,
            _ => throw new InvalidOperationException("未知の Trim オプションです。"),
        };
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
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
            ArrayPool<byte>.Shared.Return(_buffer);
            if (_currentLine != null)
                ArrayPool<byte>.Shared.Return(_currentLine);
            await _reader.DisposeAsync();
            _isDisposed = true;
        }
    }
}