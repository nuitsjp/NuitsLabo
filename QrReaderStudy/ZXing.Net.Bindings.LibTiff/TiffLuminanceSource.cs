using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

/// <summary>
/// TIFF画像から輝度情報を抽出するためのクラスです。
/// </summary>
public sealed class TiffLuminanceSource : BaseLuminanceSource
{
    /// <summary>
    /// 指定されたTIFF画像から新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="tiff">輝度情報を抽出するTIFF画像。</param>
    /// <exception cref="ArgumentException">TIFF画像が1ビットでない場合にスローされます。</exception>
    public TiffLuminanceSource(Tiff tiff)
        : base(tiff.GetWidth(), tiff.GetHeight())
    {
        var width = Width;
        var height = Height;

        // TIFF画像のビット深度を確認
        var bitsPerSampleField = tiff.GetField(TiffTag.BITSPERSAMPLE);
        if (bitsPerSampleField == null || bitsPerSampleField[0].ToInt() != 1)
            throw new ArgumentException(@"The provided TIFF image is not 1-bit.", nameof(tiff));

        var scanlineSize = tiff.ScanlineSize();
        var scanline = new byte[scanlineSize];

        // 各スキャンラインを読み取り、輝度情報を抽出
        for (var row = 0; row < height; row++)
        {
            tiff.ReadScanline(scanline, row);
            unsafe
            {
                fixed (byte* scanlinePtr = scanline)
                {
                    for (var col = 0; col < width; col++)
                    {
                        var byteIndex = col / 8;
                        var bitIndex = 7 - (col % 8); // TIFFは上位ビットから順に格納
                        var isWhite = (scanlinePtr[byteIndex] & (1 << bitIndex)) != 0;
                        luminances[row * width + col] = isWhite ? (byte)255 : (byte)0;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 指定された輝度情報、幅、高さから新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="luminances">輝度情報の配列。</param>
    /// <param name="width">画像の幅。</param>
    /// <param name="height">画像の高さ。</param>
    private TiffLuminanceSource(byte[] luminances, int width, int height)
        : base(luminances, width, height)
    {
    }

    /// <summary>
    /// 新しい輝度情報、幅、高さから新しいLuminanceSourceを作成します。
    /// </summary>
    /// <param name="newLuminances">新しい輝度情報の配列。</param>
    /// <param name="width">新しい画像の幅。</param>
    /// <param name="height">新しい画像の高さ。</param>
    /// <returns>新しいLuminanceSourceのインスタンス。</returns>
    protected override LuminanceSource CreateLuminanceSource(byte[] newLuminances, int width, int height)
    {
        return new TiffLuminanceSource(newLuminances, width, height);
    }
}
