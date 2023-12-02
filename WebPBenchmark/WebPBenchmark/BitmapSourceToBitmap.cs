
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;
using Point = System.Drawing.Point;

namespace WebPBenchmark;

[SimpleJob]
[MemoryDiagnoser]
public class BitmapSourceToBitmap
{
    private readonly BitmapSource _source = new BitmapImage(new Uri(new FileInfo("Color.jpg").FullName));


    [Benchmark(Baseline = true)]
    public Bitmap BitmapData()
    {
        var bitmap = new Bitmap(
            _source.PixelWidth,
            _source.PixelHeight,
            PixelFormat.Format32bppPArgb);
        var data = bitmap.LockBits(
            new Rectangle(Point.Empty, bitmap.Size),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppPArgb);
        try
        {
            _source.CopyPixels(
                Int32Rect.Empty,
                data.Scan0,
                data.Height * data.Stride,
                data.Stride);
            return bitmap;
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    [Benchmark]
    public Bitmap BitmapEncoderJpeg()
    {
        using var outStream = new MemoryStream();
        var enc = new JpegBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create(_source));
        enc.Save(outStream);
        return new Bitmap(outStream);
    }

    [Benchmark]
    public Bitmap BitmapEncoderBmp()
    {
        using var outStream = new MemoryStream();
        var enc = new BmpBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create(_source));
        enc.Save(outStream);
        return new Bitmap(outStream);
    }
}