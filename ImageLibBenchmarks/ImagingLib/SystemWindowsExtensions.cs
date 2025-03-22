using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace ImagingLib;

public static class SystemWindowsExtensions
{
    public const int DefaultThreshold = 75;

    /// <summary>
    /// 二値化時に利用する赤色の重み
    /// </summary>
    private const int RedFactor = (int)(0.298912 * 1024);

    /// <summary>
    /// 二値化時に利用する緑色の重み
    /// </summary>
    private const int GreenFactor = (int)(0.586611 * 1024);

    /// <summary>
    /// 二値化時に利用する青色の重み
    /// </summary>
    private const int BlueFactor = (int)(0.114478 * 1024);

    /// <summary>
    /// 大津の二値化によるしきい値を求める。
    /// </summary>
    public static unsafe int CalculateOtsuThreshold(this BitmapSource bitmapSource, bool isTiff)
    {
        // 1bppIndexedの場合はデフォルトのしきい値を返す
        // これは二値化の際に、PixelFormat.Format1bppIndexedの画像をそのまま返すため。
        // 値は何でもよいがデフォルトのしきい値を返す。
        if (isTiff)
        {
            return DefaultThreshold;
        }

        var pixels = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * 4];
        bitmapSource.CopyPixels(pixels, bitmapSource.PixelWidth * 4, 0);


        // srcからStrideを取得する
        var bitsPerPixel = bitmapSource.Format.BitsPerPixel;
        var width = bitmapSource.PixelWidth;
        var srcStride = (width * bitsPerPixel + 7) / 8;

        var grayIntPtr = Marshal.AllocCoTaskMem(srcStride * bitmapSource.PixelHeight);
        try
        {
            // グレイスケール化
            var grayScale = (byte*)grayIntPtr;
            for (var y1 = 0; y1 < bitmapSource.PixelHeight; y1++)
            {
                for (var x1 = 0; x1 < bitmapSource.PixelWidth; x1++)
                {
                    var index = (y1 * bitmapSource.PixelWidth + x1) * 4;
                    var r = pixels[index + 2];
                    var g = pixels[index + 1];
                    var b = pixels[index];

                    grayScale[srcStride * y1 + x1] = (byte)(r * RedFactor + g * GreenFactor + b * BlueFactor >> 10);
                }
            }

            // ヒストグラムの初期化
            var histogram = new int[256];
            for (var y = 0; y < bitmapSource.PixelHeight; y++)
            {
                for (var x = 0; x < bitmapSource.PixelWidth; x++)
                {
                    int grayScaleValue = grayScale[srcStride * y + x];
                    histogram[grayScaleValue]++;
                }
            }

            Debug.Write($"histogram : {string.Join(", ", histogram.Select(x => $"{x:X2}"))}");


            return histogram.OptimalThreshold(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
        }
        finally
        {
            Marshal.FreeCoTaskMem(grayIntPtr);
        }
    }
}