using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using ImageMagick;

namespace WebPBenchmark;

[SimpleJob]
[MemoryDiagnoser]
public class Binarize
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    /// <summary>
    /// 2値化しきい値
    /// </summary>
    private static readonly float Threshold = 0.75f;

    [Params(1, 3, 5, 10, 14)]
    public int N;

    [Benchmark]
    public void ImageMagickFixedThreshold()
    {
        Parallel.ForEach(new int[N], _ =>
        {
            using var magickImage = new MagickImage(_data);

            magickImage.Threshold(new Percentage(Threshold));
            magickImage.Depth = 1;

            // MagickImageからMemoryStreamに変換
            using var bitmapMemory = new MemoryStream();
            magickImage.Write(bitmapMemory, MagickFormat.Bmp);  // 一時的にBMP形式として出力
            bitmapMemory.Position = 0;
            bitmapMemory.ToArray();
        });
    }

    [Benchmark]
    public void ImageMagickOtsu()
    {
        Parallel.ForEach(new int[N], _ =>
        {
            using var magickImage = new MagickImage(_data);

            // 画像をグレースケールに変換
            magickImage.ColorSpace = ColorSpace.Gray;
            // 大津の二値化アルゴリズムを適用して二値化する
            magickImage.AutoThreshold(AutoThresholdMethod.OTSU);

            // MagickImageからMemoryStreamに変換
            using var bitmapMemory = new MemoryStream();
            magickImage.Write(bitmapMemory, MagickFormat.Bmp);  // 一時的にBMP形式として出力
            bitmapMemory.Position = 0;
            bitmapMemory.ToArray();
        });
    }

    [Benchmark]
    public void SystemDrawingFixedThreshold()
    {
        Parallel.ForEach(new int[N], _ =>
        {
            using var stream = new MemoryStream(_data);
            using Bitmap src = new(stream);
            // 変換結果のBitmapを作成する
            var dest = new Bitmap(src.Width, src.Height, PixelFormat.Format1bppIndexed);
            dest.SetResolution(src.HorizontalResolution, src.VerticalResolution);

            // Bitmapをロックし、BitmapDataを取得する
            var srcBitmapData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.WriteOnly, src.PixelFormat);
            var destBitmapData = dest.LockBits(new Rectangle(0, 0, dest.Width, dest.Height), ImageLockMode.WriteOnly, dest.PixelFormat);
            try
            {
                unsafe
                {
                    var srcPtr = (byte*)srcBitmapData.Scan0;
                    var destPtr = (byte*)destBitmapData.Scan0;

                    for (var y = 0; y < destBitmapData.Height; y++)
                    {
                        for (var x = 0; x < destBitmapData.Width; x++)
                        {
                            // 24bitカラーを256階層のグレースケールに変換し、180以上であれば白と判定する
                            if (128 <= ConvertToGrayScale(srcPtr, x, y, srcBitmapData.Stride))
                            {
                                // 二値画像は1ビットずつ格納されるため、座標は8で割ったアドレスのバイトに格納されている
                                var pos = (x >> 3) + destBitmapData.Stride * y;
                                // 該当のビットを立てることで、白にする
                                destPtr[pos] |= (byte)(0x80 >> (x & 0x7));
                            }
                        }
                    }
                }
            }
            finally
            {
                // BitmapDataのロックを解除する
                src.UnlockBits(srcBitmapData);
                dest.UnlockBits(destBitmapData);
            }

            using MemoryStream binMs = new();
            // 圧縮指定なし→LZW圧縮になる
            dest.Save(binMs, ImageFormat.Bmp);
            binMs.Position = 0;
            binMs.ToArray();
        });
    }

    [Benchmark]
    public void SystemDrawingOtsu()
    {
        Parallel.ForEach(new int[N], _ =>
        {
            using var stream = new MemoryStream(_data);
            using Bitmap src = new(stream);
            // 変換結果のBitmapを作成する
            var dest = new Bitmap(src.Width, src.Height, PixelFormat.Format1bppIndexed);
            dest.SetResolution(src.HorizontalResolution, src.VerticalResolution);

            // Bitmapをロックし、BitmapDataを取得する
            var srcBitmapData = src.LockBits(new Rectangle(0, 0, src.Width, src.Height), ImageLockMode.WriteOnly, src.PixelFormat);
            var destBitmapData = dest.LockBits(new Rectangle(0, 0, dest.Width, dest.Height), ImageLockMode.WriteOnly, dest.PixelFormat);
            try
            {
                unsafe
                {
                    var srcPtr = (byte*)srcBitmapData.Scan0;
                    var destPtr = (byte*)destBitmapData.Scan0;

                    var srcStride = srcBitmapData.Stride;

                    // 変換対象のカラー画像の情報をバイト列へ書き出す
                    var srcLength = srcStride * src.Height;
                    var srcIntPtr = Marshal.AllocCoTaskMem(srcLength);
                    try
                    {
                        // グレイスケールデータの格納先
                        var grayScale = (byte*)srcIntPtr;

                        // グレイスケール化
                        for (var y = 0; y < srcBitmapData.Height; y++)
                        {
                            for (var x = 0; x < srcBitmapData.Width; x++)
                            {
                                grayScale[x * srcStride + y] = (byte)ConvertToGrayScale(srcPtr, x, y, srcStride);
                            }
                        }

                        // 大津の二値化アルゴリズムを適用して二値化する
                        var threshold = CalculateOtsuThreshold(grayScale, srcBitmapData.Width, srcBitmapData.Height, srcStride);

                        // 二値化
                        for (var y = 0; y < destBitmapData.Height; y++)
                        {
                            for (var x = 0; x < destBitmapData.Width; x++)
                            {
                                // 二値化しきい値を超えている場合は白にする
                                if (threshold <= grayScale[x * srcStride + y])
                                {
                                    // 二値画像は1ビットずつ格納されるため、座標は8で割ったアドレスのバイトに格納されている
                                    var pos = (x >> 3) + destBitmapData.Stride * y;
                                    // 該当のビットを立てることで、白にする
                                    destPtr[pos] |= (byte)(0x80 >> (x & 0x7));
                                }
                            }
                        }
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(srcIntPtr);
                    }
                }
            }
            finally
            {
                // BitmapDataのロックを解除する
                src.UnlockBits(srcBitmapData);
                dest.UnlockBits(destBitmapData);
            }

            using MemoryStream binMs = new();
            // 圧縮指定なし→LZW圧縮になる
            dest.Save(binMs, ImageFormat.Bmp);
            binMs.Position = 0;
            binMs.ToArray();
        });
    }


    const int RedFactor = (int)(0.298912 * 1024);
    const int GreenFactor = (int)(0.586611 * 1024);
    const int BlueFactor = (int)(0.114478 * 1024);

    /// <summary>
    /// 指定された座標のピクセルのグレースケール値を求める
    /// </summary>
    /// <param name="srcPixels">変換元画像のBitmapデータのバイト配列</param>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    /// <param name="stride">スキャン幅</param>
    /// <returns>変換結果</returns>
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
                int grayScaleValue = srcPixels[x * stride + y];
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

}