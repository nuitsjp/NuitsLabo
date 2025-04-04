﻿using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using SkiaSharp;
#if NET8_0_OR_GREATER
using SixLabors.ImageSharp.PixelFormats;
#endif

namespace ImagingLib;

public static class Binarize
{
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

    public static unsafe Binary BySystemDrawing(this byte[] imageBytes, float threshold)
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(imageBytes));

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

    /// <summary>
    /// SkiaSharpを用いて画像を二値化し、Binaryを返す。
    /// 画像は通常SKBitmapは32bpp（BGRA）として読み込まれるため、各ピクセルは4バイト分のデータとなる。
    /// </summary>
    public static unsafe Binary BySkiaSharp(this byte[] imageBytes, float threshold)
    {
        // SKBitmap.Decode(byte[]) は内部で MemoryStream を生成してデコードする
        using var skBitmap = SKBitmap.Decode(imageBytes);
        if (skBitmap == null)
            throw new InvalidOperationException("画像のデコードに失敗しました。");

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

#if NET8_0_OR_GREATER
    public static unsafe Binary ByImageSharp(this byte[] imageBytes, float threshold)
    {
        // ImageSharp により Rgba32 として画像を読み込む
        using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(imageBytes);
        var width = image.Width;
        var height = image.Height;

        // 1bpp画像のストライドは、各行が4バイト境界に揃えられる
        var binStride = ((width + 7) / 8 + 3) & ~3;

        // 出力先バッファの確保（解放は呼び出し側に委ねる）
        var outBuffer = Marshal.AllocHGlobal(binStride * height);

        // バッファのゼロ初期化
        var binPtr = (byte*)outBuffer;
        for (var i = 0; i < binStride * height; i++)
        {
            binPtr[i] = 0;
        }

        // しきい値を 0～255 の範囲に合わせる
        var grayScaleThreshold = (int)(256 * threshold / 100f);

        // 各行のピクセルを走査して二値化処理を実施
        image.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < height; y++)
            {
                var rowSpan = accessor.GetRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    var pixel = rowSpan[x];
                    // グレースケール値の算出（各色の重み付け）
                    var grayScale = (pixel.R * RedFactor + pixel.G * GreenFactor + pixel.B * BlueFactor) >> 10;
                    if (grayScale >= grayScaleThreshold)
                    {
                        // 対応するバイト位置とビット位置を算出して白（1）に設定
                        var outPos = (x >> 3) + y * binStride;
                        binPtr[outPos] |= (byte)(0x80 >> (x & 0x7));
                    }
                }
            }
        });

        return new Binary(outBuffer, width, height, binStride);
    }

#endif

}