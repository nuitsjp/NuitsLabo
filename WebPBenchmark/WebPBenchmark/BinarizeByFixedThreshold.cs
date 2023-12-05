using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using System.Windows.Media.Imaging;
using ImageMagick;
using Rectangle = System.Drawing.Rectangle;
using WebPBenchmark.Extensions;

namespace WebPBenchmark;

[MemoryDiagnoser]
public class BinarizeByFixedThreshold : BaseBenchmark
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    /// <summary>
    /// 2値化しきい値
    /// </summary>
    private static readonly float Threshold = 0.75f;

    [Benchmark]
    public BitmapSource MagickNetFixedThreshold()
    {
        using var magickImage = new MagickImage(BitmapSource.ToBmpBytes());

        magickImage.Threshold(new Percentage(Threshold));
        magickImage.Depth = 1;

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
    public unsafe BitmapSource SystemDrawingFixedThreshold()
    {
        using var bitmap = BitmapSource.ToBitmap();
        var binBitmap = ToBinary(bitmap, 75f);
        return binBitmap.ToBitmapSource();
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
