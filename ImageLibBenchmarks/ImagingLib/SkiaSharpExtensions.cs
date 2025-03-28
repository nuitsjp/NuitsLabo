﻿using SkiaSharp;
using System.Runtime.InteropServices;

namespace ImagingLib;

public static class SkiaSharpExtensions
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
        using var bitmap = SKBitmap.Decode(data);
        var width = bitmap.Width;
        var height = bitmap.Height;
        var histogram = new int[256];

        // SKBitmapのピクセルデータにアクセス
        var pixmap = bitmap.PeekPixels();
        var pixels = pixmap.GetPixels();
        if (pixmap == null || pixels == IntPtr.Zero)
            throw new InvalidOperationException("ピクセルデータにアクセスできませんでした。");

        // unsafeコードで各ピクセルにアクセス
        var pixelPointer = (byte*)pixels.ToPointer();
        var rowBytes = pixmap.RowBytes;
        var colorType = pixmap.ColorType;
        for (var y = 0; y < height; y++)
        {
            var row = pixelPointer + y * rowBytes;
            for (var x = 0; x < width; x++)
            {
                var pixelOffset = x * 4; // 通常は4バイト/ピクセル（Rgba8888またはBgra8888）
                byte r, g, b;
                switch (colorType)
                {
                    case SKColorType.Rgba8888:
                        r = row[pixelOffset + 0];
                        g = row[pixelOffset + 1];
                        b = row[pixelOffset + 2];
                        break;
                    case SKColorType.Bgra8888:
                        b = row[pixelOffset + 0];
                        g = row[pixelOffset + 1];
                        r = row[pixelOffset + 2];
                        break;
                    default:
                    {
                        // 万が一他の形式の場合はGetPixelで取得（ただしパフォーマンスは低下します）
                        var color = bitmap.GetPixel(x, y);
                        r = color.Red;
                        g = color.Green;
                        b = color.Blue;
                        break;
                    }
                }

                // グレースケール化（右シフト10で1024で除算）
                var gray = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
                histogram[gray]++;
            }
        }

        var threshold = histogram.OptimalThreshold(width, height);
        var srcStride = bitmap.RowBytes; // 1行あたりのバイト数（通常 width * 4 となる）

        // SKBitmap のピクセルデータを取得（BGRA順）
        var srcPtr = (byte*)pixels.ToPointer();

        // 1bpp画像のストライドは、各行が4バイト境界に揃えられる
        var binStride = ((width + 7) / 8 + 3) & ~3;

        // 出力先メモリの確保（呼び出し側で解放）
        var outBuffer = Marshal.AllocHGlobal(binStride * height);

        // ゼロ初期化
        var binPtr = (byte*)outBuffer;
        for (var i = 0; i < binStride * height; i++)
        {
            binPtr[i] = 0;
        }

        // しきい値を0～255の範囲に合わせる
        var grayScaleThreshold = (int)(256 * (float)threshold / 100f);

        // SKBitmap は通常 32bpp（BGRA）のため、1ピクセルあたり4バイト分のデータ
        for (var y1 = 0; y1 < height; y1++)
        {
            for (var x1 = 0; x1 < width; x1++)
            {
                var pos = x1 * 4 + y1 * srcStride;
                var b1 = srcPtr[pos + 0];
                var g1 = srcPtr[pos + 1];
                var r1 = srcPtr[pos + 2];
                var grayScale = (r1 * RedFactor + g1 * GreenFactor + b1 * BlueFactor) >> 10;

                if (grayScaleThreshold <= grayScale)
                {
                    var outPos = (x1 >> 3) + y1 * binStride;
                    binPtr[outPos] |= (byte)(0x80 >> (x1 & 0x7));
                }
            }
        }

        return new Binary(outBuffer, width, height, binStride);
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

        // SKBitmapのピクセルデータにアクセス
        var pixmap = bitmap.PeekPixels();
        var pixels = pixmap.GetPixels();
        if (pixmap == null || pixels == IntPtr.Zero)
            throw new InvalidOperationException("ピクセルデータにアクセスできませんでした。");

        // unsafeコードで各ピクセルにアクセス
        var pixelPointer = (byte*)pixels.ToPointer();
        var rowBytes = pixmap.RowBytes;
        var colorType = pixmap.ColorType;
        for (var y = 0; y < height; y++)
        {
            var row = pixelPointer + y * rowBytes;
            for (var x = 0; x < width; x++)
            {
                var pixelOffset = x * 4; // 通常は4バイト/ピクセル（Rgba8888またはBgra8888）
                byte r, g, b;
                switch (colorType)
                {
                    case SKColorType.Rgba8888:
                        r = row[pixelOffset + 0];
                        g = row[pixelOffset + 1];
                        b = row[pixelOffset + 2];
                        break;
                    case SKColorType.Bgra8888:
                        b = row[pixelOffset + 0];
                        g = row[pixelOffset + 1];
                        r = row[pixelOffset + 2];
                        break;
                    default:
                    {
                        // 万が一他の形式の場合はGetPixelで取得（ただしパフォーマンスは低下します）
                        var color = bitmap.GetPixel(x, y);
                        r = color.Red;
                        g = color.Green;
                        b = color.Blue;
                        break;
                    }
                }

                // グレースケール化（右シフト10で1024で除算）
                var gray = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
                histogram[gray]++;
            }
        }

        return histogram.OptimalThreshold(width, height);
    }

    /// <summary>
    /// SkiaSharpを用いて画像を二値化し、Binaryを返す。
    /// 画像は通常SKBitmapは32bpp（BGRA）として読み込まれるため、各ピクセルは4バイト分のデータとなる。
    /// </summary>
    public static unsafe Binary ToBinary(this SKBitmap skBitmap, float threshold)
    {
        var width = skBitmap.Width;
        var height = skBitmap.Height;
        var srcStride = skBitmap.RowBytes; // 1行あたりのバイト数（通常 width * 4 となる）

        // SKBitmap のピクセルデータを取得（BGRA順）
        var srcPtr = (byte*)skBitmap.GetPixels().ToPointer();

        // 1bpp画像のストライドは、各行が4バイト境界に揃えられる
        var binStride = ((width + 7) / 8 + 3) & ~3;

        // 出力先メモリの確保（呼び出し側で解放）
        var outBuffer = Marshal.AllocHGlobal(binStride * height);

        // ゼロ初期化
        var binPtr = (byte*)outBuffer;
        for (var i = 0; i < binStride * height; i++)
        {
            binPtr[i] = 0;
        }

        // しきい値を0～255の範囲に合わせる
        var grayScaleThreshold = (int)(256 * threshold / 100f);

        // SKBitmap は通常 32bpp（BGRA）のため、1ピクセルあたり4バイト分のデータ
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var pos = x * 4 + y * srcStride;
                var b = srcPtr[pos + 0];
                var g = srcPtr[pos + 1];
                var r = srcPtr[pos + 2];
                var grayScale = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;

                if (grayScaleThreshold <= grayScale)
                {
                    var outPos = (x >> 3) + y * binStride;
                    binPtr[outPos] |= (byte)(0x80 >> (x & 0x7));
                }
            }
        }

        return new Binary(outBuffer, width, height, binStride);
    }
}