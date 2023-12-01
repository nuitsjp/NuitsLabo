using System.IO;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;
using ImageMagick;
using SixLabors.ImageSharp;
using System.Drawing.Imaging;
using Image = SixLabors.ImageSharp.Image;

namespace WebPBenchmark;

[SimpleJob]
[MemoryDiagnoser]
public class LoadWebP
{
    private readonly byte[] _data = File.ReadAllBytes("Color.webp");


    [Benchmark]
    public BitmapImage Imaging()
    {
        using var bitmapMemory = new MemoryStream(_data);

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
    public BitmapSource ImagingAdjustDpiByRenderTargetBitmap()
    {
        using var bitmapMemory = new MemoryStream(_data);

        // MemoryStreamからBitmapImageに変換
        var sourceImage = new BitmapImage();
        sourceImage.BeginInit();
        sourceImage.CacheOption = BitmapCacheOption.OnLoad;
        sourceImage.StreamSource = bitmapMemory;
        sourceImage.EndInit();
        sourceImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        // 新しいDPI値でRenderTargetBitmapを作成
        var renderTarget = new RenderTargetBitmap(
            sourceImage.PixelWidth, sourceImage.PixelHeight, 300, 300, PixelFormats.Pbgra32);

        // DrawingVisualを使用して画像をレンダリングする
        var visual = new DrawingVisual();
        using (var context = visual.RenderOpen())
        {
            context.DrawImage(sourceImage, new Rect(0, 0, sourceImage.PixelWidth, sourceImage.PixelHeight));
        }

        // 新しいRenderTargetに描画を適用
        renderTarget.Render(visual);

        return renderTarget;
    }

    [Benchmark]
    public BitmapSource ImagingAdjustDpiByDrawingByPng()
    {
        using var stream = new MemoryStream(_data);

        // MemoryStreamからBitmapImageに変換
        var source = new BitmapImage();
        source.BeginInit();
        source.CacheOption = BitmapCacheOption.OnLoad;
        source.StreamSource = stream;
        source.EndInit();
        source.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        using var bitmap = source.ToBitmap(300, 300);
        return bitmap.ToBitmapSource(ImageFormat.Png);
    }

    [Benchmark]
    public BitmapSource ImagingAdjustDpiByDrawingByJpg()
    {
        using var stream = new MemoryStream(_data);

        // MemoryStreamからBitmapImageに変換
        var source = new BitmapImage();
        source.BeginInit();
        source.CacheOption = BitmapCacheOption.OnLoad;
        source.StreamSource = stream;
        source.EndInit();
        source.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        using var bitmap = source.ToBitmap(300, 300);
        return bitmap.ToBitmapSource();
    }

    [Benchmark]
    public BitmapSource ImagingAdjustDpiByDrawingByBmp()
    {
        using var stream = new MemoryStream(_data);

        // MemoryStreamからBitmapImageに変換
        var source = new BitmapImage();
        source.BeginInit();
        source.CacheOption = BitmapCacheOption.OnLoad;
        source.StreamSource = stream;
        source.EndInit();
        source.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        using var bitmap = source.ToBitmap(300, 300);
        return bitmap.ToBitmapSource(ImageFormat.Bmp);
    }

    [Benchmark]
    public BitmapImage MagickDotNet()
    {
        using var magickImage = new MagickImage(_data);
        var stream = new MemoryStream();
        magickImage.Write(stream, MagickFormat.Bmp);
        stream.Position = 0;

        // MemoryStreamからBitmapImageに変換
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        return bitmapImage;
    }

    [Benchmark]
    public BitmapImage ImageSharp()
    {
        using var image = Image.Load(_data);
        image.Metadata.HorizontalResolution = 300;
        image.Metadata.VerticalResolution = 300;
        var stream = new MemoryStream();
        image.SaveAsBmp(stream);
        stream.Position = 0;

        // MemoryStreamからBitmapImageに変換
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        return bitmapImage;
    }
}
