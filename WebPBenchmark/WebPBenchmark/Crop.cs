using ImageMagick;
using System.IO;
using BenchmarkDotNet.Attributes;
using System.Drawing.Imaging;
using System.Drawing;

namespace WebPBenchmark;

[SimpleJob]
public class Crop
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    [Benchmark]
    public byte[] MagickImage()
    {
        using var magickImage = new MagickImage(_data);

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
        using var inputStream = new MemoryStream(_data);
        using var originalImage = new Bitmap(inputStream);

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
}