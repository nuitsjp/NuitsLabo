using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;
using System.IO;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace WebPBenchmark;

[SimpleJob]
public class CreateThumbnail
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    [Benchmark]
    public async Task<BitmapImage> MagickImage()
    {
        using var magickImage = new MagickImage(_data);

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
        using var inputStream = new MemoryStream(_data);
        using var originalImage = new Bitmap(inputStream);

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

}