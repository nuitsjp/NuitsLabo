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

        // GetPixelsUnsafe() を使い、BGR の順（3チャネル）でピクセルデータを一括取得
        using var pixels = image.GetPixelsUnsafe();
        // ToByteArray() で全ピクセルのバイト配列を取得（各ピクセルは B, G, R の順に格納）
        var pixelBytes = pixels.ToByteArray(0, 0, width, height, "BGR");
        var gray = new byte[width * height];
        var histogram = new int[256];

        // 各ピクセルは3バイトずつ格納されているので、ループで走査
        for (int i = 0, pixelIndex = 0; i < pixelBytes.Length; i += 3, pixelIndex++)
        {
            byte b = pixelBytes[i];
            byte g = pixelBytes[i + 1];
            byte r = pixelBytes[i + 2];
            int grayValue = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
            gray[pixelIndex] = (byte)grayValue;
            histogram[grayValue]++;
        }

        // OptimalThresholdは各自実装済みの拡張メソッドと仮定
        var threshold = histogram.OptimalThreshold((int)width, (int)height);
        var grayScaleThreshold = (int)(256 * (float)threshold / 100f);

        // 1bpp画像のストライド計算（各行は4バイト境界に揃える）
        var binStride = (((int)width + 7) / 8 + 3) & ~3;
        var outBuffer = Marshal.AllocHGlobal(binStride * (int)height);
        unsafe
        {
            byte* binPtr = (byte*)outBuffer.ToPointer();
            for (int i = 0; i < binStride * height; i++)
            {
                binPtr[i] = 0;
            }
        }

        // 二値化処理
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int grayValue = gray[y * width + x];
                if (grayValue >= grayScaleThreshold)
                {
                    int outPos = (x >> 3) + y * binStride;
                    int bitIndex = x & 7;
                    unsafe
                    {
                        byte* p = (byte*)outBuffer.ToPointer();
                        p[outPos] |= (byte)(0x80 >> bitIndex);
                    }
                }
            }
        }

        return new Binary(outBuffer, (int)width, (int)height, binStride);
    }
}