using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

/// <summary>
/// TIFF画像に対する拡張メソッドを提供するクラスです。
/// </summary>
public static class TiffExtensions
{
    /// <summary>
    /// TIFF画像の幅を取得します。
    /// </summary>
    /// <param name="tiff">幅を取得するTIFF画像。</param>
    /// <returns>TIFF画像の幅。</returns>
    /// <exception cref="ArgumentException">TIFF画像にIMAGEWIDTHフィールドが含まれていない場合にスローされます。</exception>
    public static int GetWidth(this Tiff tiff)
    {
        // TIFF画像からIMAGEWIDTHフィールドを取得
        var widthField = tiff.GetField(TiffTag.IMAGEWIDTH);
        // IMAGEWIDTHフィールドが存在しない場合は例外をスロー
        return widthField == null
            ? throw new ArgumentException("TIFF image does not contain IMAGEWIDTH field.", nameof(tiff))
            : widthField[0].ToInt();
    }

    /// <summary>
    /// TIFF画像の高さを取得します。
    /// </summary>
    /// <param name="tiff">高さを取得するTIFF画像。</param>
    /// <returns>TIFF画像の高さ。</returns>
    /// <exception cref="ArgumentException">TIFF画像にIMAGELENGTHフィールドが含まれていない場合にスローされます。</exception>
    public static int GetHeight(this Tiff tiff)
    {
        // TIFF画像からIMAGELENGTHフィールドを取得
        var heightField = tiff.GetField(TiffTag.IMAGELENGTH);
        // IMAGELENGTHフィールドが存在しない場合は例外をスロー
        return heightField == null
            ? throw new ArgumentException("TIFF image does not contain IMAGELENGTH field.", nameof(tiff))
            : heightField[0].ToInt();
    }
}
