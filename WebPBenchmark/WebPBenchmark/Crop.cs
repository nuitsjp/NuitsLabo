using ImageMagick;
using System.IO;
using BenchmarkDotNet.Attributes;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;

namespace WebPBenchmark;

[ShortRunJob]
[MemoryDiagnoser]
public class Crop
{
    private readonly byte[] _webp = File.ReadAllBytes("Color.webp");
    private readonly byte[] _jpeg = File.ReadAllBytes("Color.jpg");

    [Params("Jpeg", "WebP")] 
    public string Format { get; set; } = string.Empty;

    private byte[] Data => Format == "WebP" ? _webp : _jpeg;

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
        using var inputStream = new MemoryStream(Data);
        using var originalImage = Format == "Jpeg"
            ? new Bitmap(inputStream)
            : LoadWebP();

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

    private Bitmap LoadWebP()
    {
        using var bitmapMemory = new MemoryStream(Data);

        // MemoryStreamからBitmapImageに変換
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = bitmapMemory;
        bitmapImage.EndInit();
        bitmapImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        return bitmapImage.ToBitmap(300, 300);
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
}