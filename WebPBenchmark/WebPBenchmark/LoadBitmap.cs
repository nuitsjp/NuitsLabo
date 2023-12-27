using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;
using ImageMagick;
using SixLabors.ImageSharp.PixelFormats;
using WebPBenchmark.Extensions;
using Image = SixLabors.ImageSharp.Image;

namespace WebPBenchmark;

[MemoryDiagnoser]
public class LoadBitmap : BaseBenchmark
{
    [Benchmark]
    public Bitmap SystemDrawing()
    {
        if (IsWebP)
        {
            using var bitmapMemory = new MemoryStream(Data);

            // MemoryStreamからBitmapImageに変換
            var sourceImage = new BitmapImage();
            sourceImage.BeginInit();
            sourceImage.CacheOption = BitmapCacheOption.OnLoad;
            sourceImage.StreamSource = bitmapMemory;
            sourceImage.EndInit();
            sourceImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです
            return sourceImage.ToBitmap();
        }
        else
        {
            return new Bitmap(new MemoryStream(Data));
        }
    }


    [Benchmark]
    public Bitmap MagickDotNet()
    {
        using var magickImage = new MagickImage(Data);
        var stream = new MemoryStream();
        magickImage.Write(stream, MagickFormat.Bmp);
        stream.Position = 0;

        return new Bitmap(stream);
    }

    [Benchmark]
    public Bitmap ImageSharp()
    {
        using var source = new MemoryStream(Data);
        using var image = Image.Load<Rgba32>(source);

        // ImageSharpの画像をMemoryStreamに書き込む
        using var output = new MemoryStream();
        image.Save(output, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
        output.Position = 0;

        // MemoryStreamからSystem.Drawing.Bitmapを作成
        return new Bitmap(output);
    }
}