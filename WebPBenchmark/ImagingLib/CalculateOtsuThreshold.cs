using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SkiaSharp;
using System.Diagnostics;
using System.Windows.Media.Imaging;

#if NET8_0_OR_GREATER
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
#endif

namespace ImagingLib;

public static class CalculateOtsuThreshold
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
    
    public static unsafe int CalculateOtsu(this Bitmap src)
    {
        // 1bppIndexedの場合はデフォルトのしきい値を返す
        // これは二値化の際に、PixelFormat.Format1bppIndexedの画像をそのまま返すため。
        // 値は何でもよいがデフォルトのしきい値を返す。
        if (src.PixelFormat == PixelFormat.Format1bppIndexed)
        {
            return DefaultThreshold;
        }

        // Bitmapをロックし、BitmapDataを取得する
        var srcBitmapData = src.LockBits(new System.Drawing.Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, src.PixelFormat);
        try
        {
            var srcStride = srcBitmapData.Stride;
            // GrayScaleに変換
            var srcPtr = (byte*)srcBitmapData.Scan0;
            var srcStride1 = srcBitmapData.Stride;
            var grayIntPtr = Marshal.AllocCoTaskMem(srcStride1 * srcBitmapData.Height);
            try
            {
                // グレイスケール化
                var grayScale = (byte*)grayIntPtr;
                for (var y1 = 0; y1 < srcBitmapData.Height; y1++)
                {
                    for (var x1 = 0; x1 < srcBitmapData.Width; x1++)
                    {
                        var position = x1 * 3 + srcStride1 * y1;
                        var b = srcPtr[position + 0];
                        var g = srcPtr[position + 1];
                        var r = srcPtr[position + 2];
                        grayScale[srcStride1 * y1 + x1] = (byte)(r * RedFactor + g * GreenFactor + b * BlueFactor >> 10);
                    }
                }

                // ヒストグラムの初期化
                var histogram = new int[256];
                for (var y = 0; y < srcBitmapData.Height; y++)
                {
                    for (var x = 0; x < srcBitmapData.Width; x++)
                    {
                        int grayScaleValue = grayScale[srcStride * y + x];
                        histogram[grayScaleValue]++;
                    }
                }

                // 画像全体のピクセル数
                var totalPixels = srcBitmapData.Width * srcBitmapData.Height;
                float sumOfHistogram = 0;

                // ヒストグラムの合計を計算
                for (var i = 0; i < 256; i++)
                {
                    sumOfHistogram += i * histogram[i];
                }

                float sumB = 0; // 背景のピクセルの合計
                var weightBackground = 0; // 背景の重み

                float maxVariance = 0; // 最大分散
                var optimalThreshold = 0; // 最適なしきい値

                // 各階調に対して最適なしきい値を計算
                for (var i = 0; i < 256; i++)
                {
                    weightBackground += histogram[i];
                    if (weightBackground == 0) continue;

                    var weightForeground = totalPixels - weightBackground; // 前景の重み
                    if (weightForeground == 0) break;

                    sumB += i * histogram[i];
                    var meanBackground = sumB / weightBackground;
                    var meanForeground = (sumOfHistogram - sumB) / weightForeground;

                    // 背景と前景の分散を計算
                    var varianceBetween = weightBackground * (float)weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);
                    if (varianceBetween > maxVariance)
                    {
                        maxVariance = varianceBetween;
                        optimalThreshold = i;
                    }
                }

                return (int)(optimalThreshold / 256f * 100);
            }
            finally
            {
                Marshal.FreeCoTaskMem(grayIntPtr);
            }
        }
        finally
        {
            // BitmapDataのロックを解除する
            src.UnlockBits(srcBitmapData);
        }
    }

    /// <summary>
    /// 大津の二値化によるしきい値を求める（SkiaSharp版）。
    /// 画像が既に二値の場合（SKColorType.Index8と仮定）にはデフォルトのしきい値（ここでは50）を返します。
    /// </summary>
    /// <param name="bitmap">処理対象のSKBitmap</param>
    /// <returns>最適なしきい値（0～100の割合）</returns>
    public static unsafe int CalculateOtsu(this SKBitmap bitmap)
    {
        var width = bitmap.Width;
        var height = bitmap.Height;
        var totalPixels = width * height;
        var histogram = new int[256];
        long sumHistogram = 0;

        // グレースケール変換用の係数（各値は元の0.299, 0.587, 0.114を1024倍したもの）
        const int RedFactor = 306;
        const int GreenFactor = 601;
        const int BlueFactor = 117;

        // SKBitmapのピクセルデータにアクセス
        var pixmap = bitmap.PeekPixels();
        if (pixmap == null || pixmap.GetPixels() == IntPtr.Zero)
            throw new InvalidOperationException("ピクセルデータにアクセスできませんでした。");

        // unsafeコードで各ピクセルにアクセス
        var pixels = (byte*)pixmap.GetPixels().ToPointer();
        var rowBytes = pixmap.RowBytes;
        for (var y = 0; y < height; y++)
        {
            var row = pixels + y * rowBytes;
            for (var x = 0; x < width; x++)
            {
                var pixelOffset = x * 4; // 通常は4バイト/ピクセル（Rgba8888またはBgra8888）
                byte r, g, b;
                if (pixmap.ColorType == SKColorType.Rgba8888)
                {
                    r = row[pixelOffset + 0];
                    g = row[pixelOffset + 1];
                    b = row[pixelOffset + 2];
                }
                else if (pixmap.ColorType == SKColorType.Bgra8888)
                {
                    b = row[pixelOffset + 0];
                    g = row[pixelOffset + 1];
                    r = row[pixelOffset + 2];
                }
                else
                {
                    // 万が一他の形式の場合はGetPixelで取得（ただしパフォーマンスは低下します）
                    var color = bitmap.GetPixel(x, y);
                    r = color.Red;
                    g = color.Green;
                    b = color.Blue;
                }

                // グレースケール化（右シフト10で1024で除算）
                var gray = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
                histogram[gray]++;
            }
        }

        // 画像全体のグレースケール値の合計（各輝度値×出現回数）
        for (var i = 0; i < 256; i++)
        {
            sumHistogram += i * histogram[i];
        }

        long sumB = 0; // 背景側のグレースケール値の累積
        var weightBackground = 0; // 背景のピクセル数
        double maxVariance = 0;
        var optimalThreshold = 0;

        // 各グレースケール階調について大津の手法で最適なしきい値を算出
        for (var t = 0; t < 256; t++)
        {
            weightBackground += histogram[t];
            if (weightBackground == 0) continue;

            var weightForeground = totalPixels - weightBackground;
            if (weightForeground == 0) break;

            sumB += t * histogram[t];
            var meanBackground = (double)sumB / weightBackground;
            var meanForeground = (double)(sumHistogram - sumB) / weightForeground;

            var varianceBetween = weightBackground * (double)weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);
            if (varianceBetween > maxVariance)
            {
                maxVariance = varianceBetween;
                optimalThreshold = t;
            }
        }

        // 元コードでは最終的にしきい値を (optimalThreshold / 256f * 100) として返す
        return (int)(optimalThreshold / 256f * 100f);
    }

    /// <summary>
    /// 大津の二値化によるしきい値を求める。
    /// </summary>
    public static unsafe int CalculateOtsu(this BitmapSource bitmapSource, bool isTiff)
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


            return OptimalThreshold(bitmapSource.PixelWidth, bitmapSource.PixelHeight, histogram);
        }
        finally
        {
            Marshal.FreeCoTaskMem(grayIntPtr);
        }
    }

    private static int OptimalThreshold(int width, int height, int[] histogram)
    {
        // 画像全体のピクセル数
        var totalPixels = width * height;
        float sumOfHistogram = 0;

        // ヒストグラムの合計を計算
        for (var i = 0; i < 256; i++)
        {
            sumOfHistogram += i * histogram[i];
        }

        float sumB = 0; // 背景のピクセルの合計
        var weightBackground = 0; // 背景の重み

        float maxVariance = 0; // 最大分散
        var optimalThreshold = 0; // 最適なしきい値

        // 各階調に対して最適なしきい値を計算
        for (var i = 0; i < 256; i++)
        {
            weightBackground += histogram[i];
            if (weightBackground == 0) continue;

            var weightForeground = totalPixels - weightBackground; // 前景の重み
            if (weightForeground == 0) break;

            sumB += i * histogram[i];
            var meanBackground = sumB / weightBackground;
            var meanForeground = (sumOfHistogram - sumB) / weightForeground;

            // 背景と前景の分散を計算
            var varianceBetween = weightBackground * (float)weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);
            if (varianceBetween > maxVariance)
            {
                maxVariance = varianceBetween;
                optimalThreshold = i;
            }
        }

        return (int)(optimalThreshold / 256f * 100);
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Otsu の二値化により最適なしきい値を求める (百分率で返す)。
    /// ImageSharp は 1bpp をサポートしないため、そのチェックは行っていません。
    /// </summary>
    /// <param name="image">対象の Image&lt;Rgba32&gt; 画像</param>
    /// <returns>最適なしきい値 (Threshold 型にキャスト)</returns>
    public static int CalculateOtsu(this Image<Rgba32> image)
    {
        var width = image.Width;
        var height = image.Height;
        var totalPixels = width * height;
        var histogram = new int[256];

        // 画像全体を走査し、グレイスケール値に変換しながらヒストグラムを作成
        image.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < height; y++)
            {
                var pixelRow = accessor.GetRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    var pixel = pixelRow[x];
                    // (R * 306 + G * 601 + B * 117) >> 10 でグレイスケール値を算出
                    var gray = (pixel.R * RedFactor + pixel.G * GreenFactor + pixel.B * BlueFactor) >> 10;
                    histogram[gray]++;
                }
            }
        });

        return OptimalThreshold(width, height, histogram);
    }
#endif

}