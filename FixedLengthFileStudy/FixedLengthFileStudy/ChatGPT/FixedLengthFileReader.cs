using System.Diagnostics;
using System.Text;

namespace FixedLengthFileStudy.ChatGPT;

/// <summary>
/// 固定長ファイルを読み取るためのクラスです。
/// ファイルからレコードを逐次読み取り、指定した位置とバイト数で項目を取得します。
/// マルチバイト文字列やエンコードを考慮し、高速に動作するよう設計されています。
/// </summary>
public class FixedLengthFileReader : IFixedLengthFileReader
{
    private readonly Stream _reader;
    private readonly Encoding _encoding;
    private readonly byte[] _newlineBytes;
    private readonly int _bufferSize;
    private readonly byte[] _buffer;
    private int _bufferLength;
    private int _bufferPosition;

    /// <summary>
    /// 固定長ファイルリーダーの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="reader">読み取るストリーム。</param>
    /// <param name="encoding">ファイルのエンコーディング。</param>
    /// <param name="newLine">改行文字列。</param>
    /// <param name="trim"></param>
    /// <param name="bufferSize">バッファサイズ（デフォルト: 4096バイト）。</param>
    public FixedLengthFileReader(Stream reader, Encoding encoding, string newLine, Trim trim = Trim.StartAndEnd, int bufferSize = 4096)
    {
        _reader = reader;
        _encoding = encoding;
        Trim = trim;
        _newlineBytes = encoding.GetBytes(newLine);
        _bufferSize = bufferSize;
        _buffer = new byte[bufferSize];
        _bufferLength = 0;
        _bufferPosition = 0;
    }

    public Trim Trim { get; }

    /// <summary>
    /// 次のレコードを読み取ります。
    /// </summary>
    /// <returns>次のレコードが存在する場合は true、それ以外の場合は false。</returns>
    public bool Read()
    {
        if (!FillBuffer())
        {
            return false; // ストリームの終端
        }

        // 改行を検出してスキップ
        var newlineIndex = FindNewline();
        if (newlineIndex >= 0)
        {
            _bufferLength = newlineIndex; // 現在のレコードの長さに調整
        }
        return true;
    }

    /// <summary>
    /// 指定した位置とバイト数に基づいてフィールドを取得します。
    /// </summary>
    /// <param name="index">フィールドの開始位置（バイト単位）。</param>
    /// <param name="bytes">フィールドの長さ（バイト単位）。</param>
    /// <returns>指定されたフィールドの文字列。</returns>
    public string GetField(int index, int bytes)
    {
        if (_bufferLength <= index)
        {
            throw new IndexOutOfRangeException();
        }
        if (_bufferLength < index + bytes)
        {
            throw new ArgumentOutOfRangeException();
        }

        ReadOnlySpan<byte> fieldBytes = new ReadOnlySpan<byte>(_buffer, _bufferPosition + index, bytes);
        var field = _encoding.GetString(fieldBytes);
        return Trim switch
        {
            Trim.StartAndEnd => field.Trim(),
            Trim.Start => field.TrimStart(),
            Trim.End => field.TrimEnd(),
            Trim.None => field,
            _ => throw new InvalidOperationException("未知の Trim オプションです。"),
        };
    }

    /// <summary>
    /// バッファを埋め直します。
    /// </summary>
    /// <returns>データがバッファに読み込まれた場合は true、それ以外の場合は false。</returns>
    private bool FillBuffer()
    {
        _bufferPosition = 0;
        _bufferLength = 0;

        int bytesRead = _reader.Read(_buffer, 0, _bufferSize);
        _bufferLength += bytesRead;

        return bytesRead > 0;
    }

    /// <summary>
    /// 改行文字列をバッファ内で検索します。
    /// </summary>
    /// <returns>改行文字列の開始位置が見つかった場合はそのインデックス、それ以外の場合は -1。</returns>
    private int FindNewline()
    {
        for (int i = _bufferPosition; i <= _bufferLength - _newlineBytes.Length; i++)
        {
            if (_buffer.AsSpan(i, _newlineBytes.Length).SequenceEqual(_newlineBytes))
            {
                return i;
            }
        }

        return -1; // 改行が見つからない場合
    }

    /// <summary>
    /// リソースを解放します。
    /// </summary>
    public void Dispose()
    {
        _reader.Dispose();
    }

    /// <summary>
    /// 非同期でリソースを解放します。
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _reader.DisposeAsync();
    }
}
