using ImageMagick;
using System.IO;
using BenchmarkDotNet.Attributes;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;

namespace WebPBenchmark;

[SimpleJob]
[MemoryDiagnoser]
public class Crop : BaseBenchmark
{
    [Benchmark]
    public byte[] MagickImage()
    {
        using var magickImage = new MagickImage(Data);

        // 画像の縦横比を維持しながら、指定されたサイズにリサイズ
        magickImage.Crop(new MagickGeometry(100, 200, 300, 400)
        {
            IgnoreAspectRatio = false
        });

        // MagickImageからMemoryStreamに変換
        using var stream = new MemoryStream();
        magickImage.Write(stream, MagickFormat.Bmp);  // 一時的にBMP形式として出力
        stream.Position = 0;
        return stream.ToArray();
    }

    [Benchmark]
    public byte[] SystemDrawingImage()
    {
        using var originalImage =
            IsWebP
                ? new Load().ImagingWithoutAdjustDpi().ToBitmap()
                : new Bitmap(new MemoryStream(Data));

        // 画像の縦横比を維持しながら、指定されたサイズにリサイズ
        var srcRect = new Rectangle(100, 200, 300, 400);
        var destRect = new Rectangle(0, 0, 300, 400);
        var destImage = new Bitmap(destRect.Width, destRect.Height);

        destImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

        using var graphics = Graphics.FromImage(destImage);
        graphics.DrawImage(originalImage, destRect, srcRect, GraphicsUnit.Pixel);

        // BitmapからMemoryStreamに変換
        using var stream = new MemoryStream();
        destImage.Save(stream, ImageFormat.Bmp);  // 一時的にBMP形式として出力
        stream.Position = 0;
        return stream.ToArray();
    }

    [Benchmark]
    public byte[] SystemWindowsMediaImaging()
    {
        using var stream = new MemoryStream(Data);

        // MemoryStreamからBitmapImageに変換
        var source = new BitmapImage();
        source.BeginInit();
        source.CacheOption = BitmapCacheOption.OnLoad;
        source.StreamSource = stream;
        source.EndInit();
        source.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        // クロップする領域を定義
        var cropArea = new Int32Rect(100, 200, 300, 400);

        // CroppedBitmapを使用して指定領域をクロップ
        var croppedBitmap = new CroppedBitmap(source, cropArea);

        // BMPエンコーダのインスタンスを作成
        var encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(croppedBitmap));

        // メモリストリームを使用してBMPデータを書き出し
        using var cropStream = new MemoryStream();
        encoder.Save(cropStream);
        // メモリストリームからバイト配列を取得
        return cropStream.ToArray();
    }

    private BitmapImage _bitmapImage = default!;
    [GlobalSetup(Target = nameof(SystemWindowsMediaImagingWithoutLoad))]
    public void GlobalSetupSystemWindowsMediaImagingWithoutLoad()
    {
        using var stream = new MemoryStream(Data);

        // MemoryStreamからBitmapImageに変換
        _bitmapImage = new BitmapImage();
        _bitmapImage.BeginInit();
        _bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        _bitmapImage.StreamSource = stream;
        _bitmapImage.EndInit();
        _bitmapImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです
    }

    [Benchmark]
    public byte[] SystemWindowsMediaImagingWithoutLoad()
    {
        // クロップする領域を定義
        var cropArea = new Int32Rect(100, 200, 300, 400);

        // CroppedBitmapを使用して指定領域をクロップ
        var croppedBitmap = new CroppedBitmap(_bitmapImage, cropArea);

        // BMPエンコーダのインスタンスを作成
        var encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(croppedBitmap));

        // メモリストリームを使用してBMPデータを書き出し
        using var cropStream = new MemoryStream();
        encoder.Save(cropStream);
        // メモリストリームからバイト配列を取得
        return cropStream.ToArray();
    }

}