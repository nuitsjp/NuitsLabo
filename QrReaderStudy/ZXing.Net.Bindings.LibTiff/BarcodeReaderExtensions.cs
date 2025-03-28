using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

/// <summary>
/// バーコードリーダーの拡張メソッドを提供するクラスです。
/// </summary>
public static class BarcodeReaderExtensions
{
    /// <summary>
    /// TIFF画像からバーコードをデコードします。
    /// </summary>
    /// <param name="reader">バーコードリーダーのインスタンス。</param>
    /// <param name="image">デコードするTIFF画像。</param>
    /// <returns>デコードされたバーコードの結果。</returns>
    public static Result Decode(this IBarcodeReaderGeneric reader, Tiff image)
    {
        // TIFF画像から輝度情報を取得するためのソースを作成
        var luminanceSource = new TiffLuminanceSource(image);
        // 輝度情報を使用してバーコードをデコード
        return reader.Decode(luminanceSource);
    }

    /// <summary>
    /// TIFF画像から複数のバーコードをデコードします。
    /// </summary>
    /// <param name="reader">バーコードリーダーのインスタンス。</param>
    /// <param name="image">デコードするTIFF画像。</param>
    /// <returns>デコードされた複数のバーコードの結果。</returns>
    public static Result[] DecodeMultiple(this IBarcodeReaderGeneric reader, Tiff image)
    {
        // TIFF画像から輝度情報を取得するためのソースを作成
        var luminanceSource = new TiffLuminanceSource(image);
        // 輝度情報を使用して複数のバーコードをデコード
        return reader.DecodeMultiple(luminanceSource);
    }
}
