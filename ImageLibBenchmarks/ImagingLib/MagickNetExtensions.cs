using System.Runtime.InteropServices;
using ImageMagick;

namespace ImagingLib;

public static class MagickNetExtensions
{
    public const int DefaultThreshold = 75;
    private const int RedFactor = (int)(0.298912 * 1024);
    private const int GreenFactor = (int)(0.586611 * 1024);
    private const int BlueFactor = (int)(0.114478 * 1024);

    public static Binary ToBinary(this byte[] data)
    {
        using var image = new MagickImage(data);
        var width = image.Width;
        var height = image.Height;

        // ※既にバイナリの場合の処理は必要に応じて追加してください

        // BGR順でピクセルを取得（3チャネル: B,G,R）
        var pixelData = image.GetPixelsUnsafe();
        var gray = new byte[width * height];
        var histogram = new int[256];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var pos = (y * width + x) * 3;
                var pixel = pixelData.GetPixel(x, y).ToColor()!;
                var b = pixel.B;
                var g = pixel.G;
                var r = pixel.R;
                var grayValue = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
                gray[y * width + x] = (byte)grayValue;
                histogram[grayValue]++;
            }
        }

        // OptimalThresholdは各自実装済みの拡張メソッドと仮定
        var threshold = histogram.OptimalThreshold((int)width, (int)height);
        var grayScaleThreshold = (int)(256 * (float)threshold / 100f);

        // 1bpp画像のストライド計算（各行は4バイト境界に揃える）
        var binStride = (((int)width + 7) / 8 + 3) & ~3;
        var outBuffer = Marshal.AllocHGlobal(binStride * (int)height);
        unsafe
        {
            var binPtr = (byte*)outBuffer.ToPointer();
            for (var i = 0; i < binStride * height; i++)
            {
                binPtr[i] = 0;
            }
        }

        // 二値化処理
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                int grayValue = gray[y * width + x];
                if (grayValue >= grayScaleThreshold)
                {
                    var outPos = (x >> 3) + y * binStride;
                    var bitIndex = x & 7;
                    unsafe
                    {
                        var p = (byte*)outBuffer.ToPointer();
                        p[outPos] |= (byte)(0x80 >> bitIndex);
                    }
                }
            }
        }

        return new Binary(outBuffer, (int)width, (int)height, binStride);
    }
}