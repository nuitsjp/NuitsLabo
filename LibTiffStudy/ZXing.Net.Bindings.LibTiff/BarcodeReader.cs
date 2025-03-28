using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

/// <summary>
/// TIFF画像をデコードするためのバーコードリーダークラスです。
/// </summary>
public class BarcodeReader() : BarcodeReader<Tiff>(null, DefaultCreateLuminanceSource, null)
{
    /// <summary>
    /// デフォルトの輝度ソースを作成するためのデリゲートです。
    /// </summary>
    private static readonly Func<Tiff, LuminanceSource> DefaultCreateLuminanceSource =
        (image) => new TiffLuminanceSource(image);
}
