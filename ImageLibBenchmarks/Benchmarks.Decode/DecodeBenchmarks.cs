using System.Drawing;
using System.IO;
using BenchmarkDotNet.Attributes;
using ImagingLib;
using SixLabors.ImageSharp.PixelFormats;
using SkiaSharp;

namespace Benchmarks.Decode;

public class DecodeBenchmarks : BenchmarkBase
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    /// <summary>
    /// 2値化しきい値
    /// </summary>
    private static readonly float Threshold = 0.75f;

    //[Benchmark]
    //public BitmapSource MagickNetFixedThreshold()
    //{
    //    using var magickImage = new MagickImage(BitmapSource.ToBmpBytes());

    //    magickImage.Threshold(new Percentage(Threshold));
    //    magickImage.Depth = 1;

    //    // MagickImageからMemoryStreamに変換
    //    using var stream = new MemoryStream();
    //    magickImage.Write(stream, MagickFormat.Bmp);  // 一時的にBMP形式として出力
    //    stream.Position = 0;

    //    var bitmapImage = new BitmapImage();
    //    bitmapImage.BeginInit();
    //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
    //    bitmapImage.StreamSource = stream;
    //    bitmapImage.EndInit();
    //    bitmapImage.Freeze();

    //    return bitmapImage;
    //}

    [Benchmark]
    public void SystemDrawing()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(_data));
    }


    [Benchmark]
    public void SkiaSharp()
    {
        using var bitmap = SKBitmap.Decode(_data);
    }

#if NET8_0_OR_GREATER
    [Benchmark]
    public void ImageSharp()
    {
        using var stream = new MemoryStream(_data);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);
    }
#endif
}