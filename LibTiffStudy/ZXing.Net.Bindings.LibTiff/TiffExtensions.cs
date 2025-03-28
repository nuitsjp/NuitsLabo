using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

public static class TiffExtensions
{
    public static int GetWidth(this Tiff tiff)
    {
        var widthField = tiff.GetField(TiffTag.IMAGEWIDTH);
        return widthField == null
            ? throw new ArgumentException("TIFF image does not contain IMAGEWIDTH field.", nameof(tiff))
            : widthField[0].ToInt();
    }

    public static int GetHeight(this Tiff tiff)
    {
        var heightField = tiff.GetField(TiffTag.IMAGELENGTH);
        return heightField == null
            ? throw new ArgumentException("TIFF image does not contain IMAGELENGTH field.", nameof(tiff))
            : heightField[0].ToInt();
    }
}