using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

public static class BarcodeReaderExtensions
{
    public static Result Decode(this IBarcodeReaderGeneric reader, Tiff image)
    {
        var luminanceSource = new TiffLuminanceSource(image);
        return reader.Decode(luminanceSource);
    }

    public static Result[] DecodeMultiple(this IBarcodeReaderGeneric reader, Tiff image)
    {
        var luminanceSource = new TiffLuminanceSource(image);
        return reader.DecodeMultiple(luminanceSource);
    }
}