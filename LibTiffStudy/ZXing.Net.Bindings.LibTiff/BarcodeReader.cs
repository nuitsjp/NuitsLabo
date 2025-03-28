using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

/// <summary>
/// a barcode reader class which can be used with the SKBitmap type from SkiaSharp
/// </summary>
public class BarcodeReader : BarcodeReader<Tiff>
{
    /// <summary>
    /// define a custom function for creation of a luminance source with our specialized SKBitmap-supporting class
    /// </summary>
    private static readonly Func<Tiff, LuminanceSource> defaultCreateLuminanceSource =
        (image) => new TiffLuminanceSource(image);

    /// <summary>
    /// constructor which uses a custom luminance source with SKImage support
    /// </summary>
    public BarcodeReader()
        : base(null, defaultCreateLuminanceSource, null)
    {
    }
}