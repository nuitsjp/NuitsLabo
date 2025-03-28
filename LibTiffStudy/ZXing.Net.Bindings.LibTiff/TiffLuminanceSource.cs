using BitMiracle.LibTiff.Classic;

namespace ZXing.Net.Bindings.LibTiff;

public sealed class TiffLuminanceSource : BaseLuminanceSource
{
    public TiffLuminanceSource(Tiff tiff)
        : base(tiff.GetWidth(), tiff.GetHeight())
    {
        var width = Width;
        var height = Height;

        var bitsPerSampleField = tiff.GetField(TiffTag.BITSPERSAMPLE);
        if (bitsPerSampleField == null || bitsPerSampleField[0].ToInt() != 1)
            throw new ArgumentException(@"The provided TIFF image is not 1-bit.", nameof(tiff));

        var scanlineSize = tiff.ScanlineSize();
        var scanline = new byte[scanlineSize];

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

    private TiffLuminanceSource(byte[] luminances, int width, int height)
        : base(luminances, width, height)
    {
    }

    protected override LuminanceSource CreateLuminanceSource(byte[] newLuminances, int width, int height)
    {
        return new TiffLuminanceSource(newLuminances, width, height);
    }
}