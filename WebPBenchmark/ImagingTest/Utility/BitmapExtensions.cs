using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ImagingTest.Utility;

public static class BitmapExtensions
{
    /// <summary>
    /// BitmapをBitmapSourceに変換する。
    /// </summary>
    /// <param name="bitmap"></param>
    /// <returns></returns>
    public static BitmapSource ToBitmapSource(this Bitmap bitmap)
    {
        var bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly, bitmap.PixelFormat);
        try
        {
            var bitmapSource = BitmapSource.Create(
                bitmapData.Width,
                bitmapData.Height,
                bitmap.HorizontalResolution,
                bitmap.VerticalResolution,
                Convert(bitmap.PixelFormat),
                bitmap.PixelFormat == PixelFormat.Format1bppIndexed
                    ? new BitmapPalette(new List<System.Windows.Media.Color> { Colors.Black, Colors.White })
                    : null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmapSource.Freeze();

            return bitmapSource;
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }
    }

    private static System.Windows.Media.PixelFormat Convert(PixelFormat source)
    {
        return source switch
        {
            PixelFormat.Format1bppIndexed => PixelFormats.Indexed1,
            PixelFormat.Format4bppIndexed => PixelFormats.Indexed4,
            PixelFormat.Format8bppIndexed => PixelFormats.Indexed8,
            PixelFormat.Format24bppRgb => PixelFormats.Bgr24,
            PixelFormat.Format32bppRgb => PixelFormats.Bgr32,
            PixelFormat.Format32bppArgb => PixelFormats.Bgra32,
            PixelFormat.Format32bppPArgb => PixelFormats.Pbgra32,
            PixelFormat.Format48bppRgb => PixelFormats.Rgb48,
            PixelFormat.Format64bppArgb => PixelFormats.Prgba64,
            PixelFormat.Format64bppPArgb => PixelFormats.Prgba64,
            _ => throw new NotSupportedException()
        };
    }

    /// <summary>
    /// 大津の二値化によるしきい値を求める。
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>
    public static unsafe Threshold CalculateOtsuThreshold(this Bitmap src)
    {
        // 1bppIndexedの場合はデフォルトのしきい値を返す
        // これは二値化の際に、PixelFormat.Format1bppIndexedの画像をそのまま返すため。
        // 値は何でもよいがデフォルトのしきい値を返す。
        if (src.PixelFormat == PixelFormat.Format1bppIndexed)
        {
            return Threshold.Default;
        }

        // Bitmapをロックし、BitmapDataを取得する
        var srcBitmapData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.ReadOnly, src.PixelFormat);
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

                return (Threshold)(optimalThreshold / 256f * 100);
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
    /// 大津の二値化によるしきい値を求める。
    /// </summary>
    public static unsafe Threshold CalculateOtsuThreshold(this BitmapSource bitmapSource, ImageFormat imageFormat)
    {
        // 1bppIndexedの場合はデフォルトのしきい値を返す
        // これは二値化の際に、PixelFormat.Format1bppIndexedの画像をそのまま返すため。
        // 値は何でもよいがデフォルトのしきい値を返す。
        if (imageFormat == ImageFormat.Tiff)
        {
            return Threshold.Default;
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

            // 画像全体のピクセル数
            var totalPixels = bitmapSource.PixelWidth * bitmapSource.PixelHeight;
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

            return (Threshold)(optimalThreshold / 256f * 100);
        }
        finally
        {
            Marshal.FreeCoTaskMem(grayIntPtr);
        }
    }
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
    /// 画像を自動しきい値で二値化してロードする。
    /// </summary>
    /// <param name="bitmap"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static unsafe Bitmap ToBinary(this Bitmap bitmap, Threshold threshold)
    {
        // 1bppIndexedの場合はそのまま返す
        if (bitmap.PixelFormat == PixelFormat.Format1bppIndexed)
        {
            return bitmap;
        }

        // もとのBitmapをロックする
        var bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        try
        {
            // 1bppイメージを作成する
            Bitmap binBitmap =
                new(bitmapData.Width, bitmapData.Height, PixelFormat.Format1bppIndexed);
            // 解像度を合わせる
            binBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            // 新しいBitmapをロックする
            var binBitmapData = binBitmap.LockBits(
                new Rectangle(0, 0, binBitmap.Width, binBitmap.Height),
                ImageLockMode.WriteOnly, binBitmap.PixelFormat);
            try
            {
                var srcPtr = (byte*)bitmapData.Scan0;
                var srcStride = bitmapData.Stride;
                // しきい値を計算する。
                // グレイスケール化する際に、明度は0～255の範囲で取得されるため、しきい値を0～255の範囲に合わせる。
                var grayScaleThreshold = (int)(256 * threshold.AsPrimitive() / 100f);

                //　新しい画像のピクセルデータを作成する
                var pixels = (byte*)binBitmapData.Scan0;
                for (var y = 0; y < binBitmapData.Height; y++)
                {
                    for (var x = 0; x < binBitmapData.Width; x++)
                    {
                        //　明るさがしきい値以上の時は白くする
                        var position = x * 3 + srcStride * y;
                        var b = srcPtr[position + 0];
                        var g = srcPtr[position + 1];
                        var r = srcPtr[position + 2];
                        var grayScale = r * RedFactor + g * GreenFactor + b * BlueFactor >> 10;

                        if (grayScaleThreshold <= grayScale)
                        {
                            //ピクセルデータの位置
                            var pos = (x >> 3) + binBitmapData.Stride * y;
                            //白くする
                            pixels[pos] |= (byte)(0x80 >> (x & 0x7));
                        }
                    }
                }

                return binBitmap;
            }
            catch
            {
                binBitmap.UnlockBits(binBitmapData);
                binBitmap.Dispose();
                throw;
            }
            finally
            {
                binBitmap.UnlockBits(binBitmapData);
            }
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }
    }
}