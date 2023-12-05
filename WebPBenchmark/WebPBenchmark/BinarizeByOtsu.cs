using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using System.Windows.Media.Imaging;
using ImageMagick;
using Rectangle = System.Drawing.Rectangle;

namespace WebPBenchmark;

[MemoryDiagnoser]
public class BinarizeByOtsu
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    [Benchmark]
    public BitmapSource MagickNet()
    {
        using var magickImage = new MagickImage(_data);

        // 画像をグレースケールに変換
        magickImage.ColorSpace = ColorSpace.Gray;
        // 大津の二値化アルゴリズムを適用して二値化する
        magickImage.AutoThreshold(AutoThresholdMethod.OTSU);

        // MagickImageからMemoryStreamに変換
        using var stream = new MemoryStream();
        magickImage.Write(stream, MagickFormat.Bmp);  // 一時的にBMP形式として出力
        stream.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
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

    [Benchmark]
    public BitmapSource SystemDrawingSplitCalcThreshold()
    {
        var bitmap = new Bitmap(new MemoryStream(_data));
        var threshold = CalculateOtsuThreshold(bitmap);
        var binBitmap = ToBinary(bitmap, threshold);
        return binBitmap.ToBitmapSource();
    }

    [Benchmark]
    public unsafe BitmapSource SystemDrawing()
    {
        var bitmap = new Bitmap(new MemoryStream(_data));
        // 1bppIndexedの場合はそのまま返す
        if (bitmap.PixelFormat == PixelFormat.Format1bppIndexed)
        {
            return bitmap.ToBitmapSource();
        }

        var srcBitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
            bitmap.PixelFormat);
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
                for (var y = 0; y < srcBitmapData.Height; y++)
                {
                    for (var x = 0; x < srcBitmapData.Width; x++)
                    {
                        var position = x * 3 + srcStride1 * y;
                        var b = srcPtr[position + 0];
                        var g = srcPtr[position + 1];
                        var r = srcPtr[position + 2];
                        grayScale[srcStride1 * y + x] =
                            (byte)((r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10);
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

                    sumB += (float)(i * histogram[i]);
                    var meanBackground = sumB / weightBackground;
                    var meanForeground = (sumOfHistogram - sumB) / weightForeground;

                    // 背景と前景の分散を計算
                    var varianceBetween = (float)weightBackground * (float)weightForeground *
                                          (meanBackground - meanForeground) * (meanBackground - meanForeground);
                    if (varianceBetween > maxVariance)
                    {
                        maxVariance = varianceBetween;
                        optimalThreshold = i;
                    }
                }

                // 1bppイメージを作成する
                Bitmap binBitmap = new(srcBitmapData.Width, srcBitmapData.Height, PixelFormat.Format1bppIndexed);
                // 解像度を合わせる
                binBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

                // 新しいBitmapをロックする
                var binBitmapData = binBitmap.LockBits(
                    new Rectangle(0, 0, binBitmap.Width, binBitmap.Height),
                    ImageLockMode.WriteOnly, binBitmap.PixelFormat);
                try
                {
                    //　新しい画像のピクセルデータを作成する
                    var pixels = (byte*)binBitmapData.Scan0;
                    for (var y = 0; y < binBitmapData.Height; y++)
                    {
                        for (var x = 0; x < binBitmapData.Width; x++)
                        {
                            //　明るさがしきい値以上の時は白くする
                            var grayScale1 = grayScale[srcStride1 * y + x];
                            if (optimalThreshold <= grayScale1)
                            {
                                //ピクセルデータの位置
                                var pos = (x >> 3) + binBitmapData.Stride * y;
                                //白くする
                                pixels[pos] |= (byte)(0x80 >> (x & 0x7));
                            }
                        }
                    }

                    return binBitmap.ToBitmapSource();
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
                Marshal.FreeCoTaskMem(grayIntPtr);
            }
        }
        finally
        {
            // BitmapDataのロックを解除する
            bitmap.UnlockBits(srcBitmapData);
        }
    }

    /// <summary>
    /// 大津の二値化によるしきい値を求める。
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>
    public static unsafe float CalculateOtsuThreshold(Bitmap src)
    {
        // 1bppIndexedの場合はデフォルトのしきい値を返す
        // これは二値化の際に、PixelFormat.Format1bppIndexedの画像をそのまま返すため。
        // 値は何でもよいがデフォルトのしきい値を返す。
        if (src.PixelFormat == PixelFormat.Format1bppIndexed)
        {
            return 75f;
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
                        grayScale[srcStride1 * y1 + x1] = (byte)((r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10);
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

                    sumB += (float)(i * histogram[i]);
                    var meanBackground = sumB / weightBackground;
                    var meanForeground = (sumOfHistogram - sumB) / weightForeground;

                    // 背景と前景の分散を計算
                    var varianceBetween = (float)weightBackground * (float)weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);
                    if (varianceBetween > maxVariance)
                    {
                        maxVariance = varianceBetween;
                        optimalThreshold = i;
                    }
                }

                return optimalThreshold / 256f * 100;
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

    private static unsafe int ConvertToGrayScale(byte* srcPixels, int x, int y, int stride)
    {
        var position = x * 3 + stride * y;
        var b = srcPixels[position + 0];
        var g = srcPixels[position + 1];
        var r = srcPixels[position + 2];

        return (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
    }

    private static unsafe int CalculateOtsuThreshold(byte* srcPixels, int width, int height, int stride)
    {
        // ヒストグラムの初期化
        var histogram = new int[256];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                int grayScaleValue = srcPixels[stride * y + x];
                histogram[grayScaleValue]++;
            }
        }

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

            sumB += (float)(i * histogram[i]);
            var meanBackground = sumB / weightBackground;
            var meanForeground = (sumOfHistogram - sumB) / weightForeground;

            // 背景と前景の分散を計算
            var varianceBetween = (float)weightBackground * (float)weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);
            if (varianceBetween > maxVariance)
            {
                maxVariance = varianceBetween;
                optimalThreshold = i;
            }
        }

        return optimalThreshold;
    }

    /// <summary>
    /// 画像を自動しきい値で二値化してロードする。
    /// </summary>
    /// <param name="bitmap"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static unsafe Bitmap ToBinary(Bitmap bitmap, float threshold)
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
                var grayScaleThreshold = (int)(256 * threshold / 100f);

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
                        var grayScale = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;

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
