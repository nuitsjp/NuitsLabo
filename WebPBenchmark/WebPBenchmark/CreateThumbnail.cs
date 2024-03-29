﻿using System.Drawing;
using ImageMagick;
using System.IO;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;
using System.Windows.Media;
using WebPBenchmark.Extensions;

namespace WebPBenchmark;

[MemoryDiagnoser]
public class CreateThumbnail : BaseBenchmark
{
    [Benchmark]
    public async Task<BitmapImage> MagickImage()
    {
        using var magickImage = new MagickImage(Data);

        // 画像の縦横比を維持しながら、指定されたサイズにリサイズ
        magickImage.Resize(new MagickGeometry(300, 300) { IgnoreAspectRatio = false });

        // MagickImageからMemoryStreamに変換
        using var bitmapMemory = new MemoryStream();
        await magickImage.WriteAsync(bitmapMemory, MagickFormat.Bmp);  // 一時的にBMP形式として出力
        bitmapMemory.Position = 0;

        // MemoryStreamからBitmapImageに変換
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = bitmapMemory;
        bitmapImage.EndInit();
        bitmapImage.Freeze();  // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        return bitmapImage;
    }

    [Benchmark]
    public BitmapSource SystemDrawing()
    {
        using var originalImage = 
            IsWebP 
                ? LoadToBitmapImage().ToBitmap()
                : new Bitmap(new MemoryStream(Data));

        // 画像の縦横比を維持しながら、指定されたサイズにリサイズ
        int destWidth, destHeight;
        if (originalImage.Width > originalImage.Height)
        {
            destWidth = 300;
            destHeight = (int)(originalImage.Height / (double)originalImage.Width * 300);
        }
        else
        {
            destHeight = 300;
            destWidth = (int)(originalImage.Width / (double)originalImage.Height * 300);
        }

        var resizedImage = new Bitmap(destWidth, destHeight);
        using (var graphics = Graphics.FromImage(resizedImage))
        {
            graphics.DrawImage(originalImage, 0, 0, destWidth, destHeight);
        }

        return resizedImage.ToBitmapSource();
    }

    private BitmapImage LoadToBitmapImage()
    {
        using var bitmapMemory = new MemoryStream(Data);

        // MemoryStreamからBitmapImageに変換
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = bitmapMemory;
        bitmapImage.EndInit();
        bitmapImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        return bitmapImage;

    }

    [Benchmark]
    public BitmapSource SystemWindowsMediaImaging()
    {
        var thumbnail = SystemWindowsMediaImagingWithoutAdjust();

        using var bitmap = thumbnail.ToBitmap(300, 300);
        return bitmap.ToBitmapSource();
    }

    [Benchmark]
    public BitmapSource SystemWindowsMediaImagingWithoutAdjust()
    {
        var size = 300;

        using var stream = new MemoryStream(Data);

        // MemoryStreamからBitmapImageに変換
        var source = new BitmapImage();
        source.BeginInit();
        source.CacheOption = BitmapCacheOption.OnLoad;
        source.StreamSource = stream;
        source.EndInit();
        source.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        var scaleX = size / (double)source.PixelWidth;
        var scaleY = size / (double)source.PixelHeight;
        var scale = Math.Min(scaleX, scaleY);

        // スケールが1より大きい場合は、元のサイズを維持
        scale = (scale > 1) ? 1 : scale;

        // スケーリングトランスフォームを使用してサムネイルを生成
        var transform = new ScaleTransform(scale, scale);
        var thumbnail = new TransformedBitmap(source, transform);

        // Freezeメソッドを呼び出して、サムネイルを変更不可能にする（必要に応じて）
        thumbnail.Freeze();

        return thumbnail;
    }
}
