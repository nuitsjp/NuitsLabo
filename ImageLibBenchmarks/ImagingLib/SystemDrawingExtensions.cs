using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;

namespace ImagingLib;

public static class SystemDrawingExtensions
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

    public static unsafe Binary ToBinary(this byte[] data)
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(data));

        // Bitmapをロックし、BitmapDataを取得する
        // すでに1bppIndexedの場合は、ロックしたデータをコピーして返す
        if (bitmap.PixelFormat == PixelFormat.Format1bppIndexed)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);
            try
            {
                var height = bitmapData.Height;
                var stride = bitmapData.Stride;
                // 出力用メモリを確保（呼び出し側で解放）
                var outBuffer = Marshal.AllocHGlobal(stride * height);
                var srcPtr1 = (byte*)bitmapData.Scan0;
                var destPtr = (byte*)outBuffer;
                for (var i = 0; i < stride * height; i++)
                {
                    destPtr[i] = srcPtr1[i];
                }
                return new Binary(
                    outBuffer,
                    bitmapData.Width,
                    bitmapData.Height,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }


        var srcBitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        try
        {
            var srcStride = srcBitmapData.Stride;
            // GrayScaleに変換
            var srcPtr = (byte*)srcBitmapData.Scan0;
            var srcStride1 = srcBitmapData.Stride;
            var grayIntPtr = Marshal.AllocCoTaskMem(srcStride1 * srcBitmapData.Height);
            int threshold;
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


                threshold = histogram.OptimalThreshold(srcBitmapData.Width, srcBitmapData.Height);
            }
            finally
            {
                Marshal.FreeCoTaskMem(grayIntPtr);
            }

            var width = srcBitmapData.Width;
            var height = srcBitmapData.Height;

            // 1bpp画像のストライドは、各行が4バイト境界に揃えられるため次式で計算
            var binStride = ((width + 7) / 8 + 3) & ~3;

            // 出力先メモリの確保（解放は呼び出し側に委ねる）
            var outBuffer = Marshal.AllocHGlobal(binStride * height);

            // 確保したメモリ領域をゼロで初期化
            var binPtr = (byte*)outBuffer;
            for (var i = 0; i < binStride * height; i++)
            {

                binPtr[i] = 0;
            }

            // しきい値を0～255の範囲に合わせる
            var grayScaleThreshold = (int)(256 * (float)threshold / 100f);

            var srcPtr2 = (byte*)srcBitmapData.Scan0;
            for (var y2 = 0; y2 < height; y2++)
            {
                for (var x2 = 0; x2 < width; x2++)
                {
                    var pos = x2 * 3 + y2 * srcStride;
                    var b1 = srcPtr2[pos + 0];
                    var g1 = srcPtr2[pos + 1];
                    var r1 = srcPtr2[pos + 2];
                    var grayScale1 = (r1 * RedFactor + g1 * GreenFactor + b1 * BlueFactor) >> 10;

                    if (grayScaleThreshold <= grayScale1)
                    {
                        // 書き込み先メモリの該当バイト位置を算出し、対応するビットをONにする
                        var outPos = (x2 >> 3) + y2 * binStride;
                        binPtr[outPos] |= (byte)(0x80 >> (x2 & 0x7));
                    }
                }
            }

            return new Binary(
                outBuffer,
                width,
                height,
                binStride);
        }
        finally
        {
            bitmap.UnlockBits(srcBitmapData);
        }
    }

    /// <summary>
    /// 大津の二値化によるしきい値を求める。
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>
    public static unsafe int CalculateOtsuThreshold(this Bitmap src)
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


                return histogram.OptimalThreshold(srcBitmapData.Width, srcBitmapData.Height);
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

    public static unsafe Binary ToBinary(this Bitmap bitmap, float threshold)
    {
        // すでに1bppIndexedの場合は、ロックしたデータをコピーして返す
        if (bitmap.PixelFormat == PixelFormat.Format1bppIndexed)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);
            try
            {
                var height = bitmapData.Height;
                var stride = bitmapData.Stride;
                // 出力用メモリを確保（呼び出し側で解放）
                var outBuffer = Marshal.AllocHGlobal(stride * height);
                var srcPtr = (byte*)bitmapData.Scan0;
                var destPtr = (byte*)outBuffer;
                for (var i = 0; i < stride * height; i++)
                {
                    destPtr[i] = srcPtr[i];
                }
                return new Binary(
                    outBuffer,
                    bitmapData.Width,
                    bitmapData.Height,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        // 元のBitmapを24bppRgbとしてロック
        var srcData = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        try
        {
            var width = srcData.Width;
            var height = srcData.Height;
            var srcStride = srcData.Stride;

            // 1bpp画像のストライドは、各行が4バイト境界に揃えられるため次式で計算
            var binStride = ((width + 7) / 8 + 3) & ~3;

            // 出力先メモリの確保（解放は呼び出し側に委ねる）
            var outBuffer = Marshal.AllocHGlobal(binStride * height);

            // 確保したメモリ領域をゼロで初期化
            var binPtr = (byte*)outBuffer;
            for (var i = 0; i < binStride * height; i++)
            {

                binPtr[i] = 0;
            }

            // しきい値を0～255の範囲に合わせる
            var grayScaleThreshold = (int)(256 * threshold / 100f);

            var srcPtr = (byte*)srcData.Scan0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var pos = x * 3 + y * srcStride;
                    var b = srcPtr[pos + 0];
                    var g = srcPtr[pos + 1];
                    var r = srcPtr[pos + 2];
                    var grayScale = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;

                    if (grayScaleThreshold <= grayScale)
                    {
                        // 書き込み先メモリの該当バイト位置を算出し、対応するビットをONにする
                        var outPos = (x >> 3) + y * binStride;
                        binPtr[outPos] |= (byte)(0x80 >> (x & 0x7));
                    }
                }
            }

            return new Binary(
                outBuffer,
                width,
                height,
                binStride);
        }
        finally
        {
            bitmap.UnlockBits(srcData);
        }
    }
}