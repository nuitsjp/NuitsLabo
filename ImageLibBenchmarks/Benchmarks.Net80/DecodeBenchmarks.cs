using System.Drawing;
using System.IO;
using BenchmarkDotNet.Attributes;
using BitMiracle.LibTiff.Classic;
using ImageMagick;
using ImagingLib;
using SkiaSharp;

namespace Benchmarks.Net80;

[MemoryDiagnoser]
//[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1)]
[SimpleJob]
public class DecodeBenchmarks : BenchmarkBase
{
    [Benchmark]
    public void SystemDrawing()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(Data));
    }


    [Benchmark]
    public void SkiaSharp()
    {
        using var bitmap = SKBitmap.Decode(Data);
    }

    // LibTiffは純粋にデコードだけ評価するのが難しいので除外
    //[Benchmark]
    //public void LibTiff()
    //{
    //    using var ms = new MemoryStream(Data);
    //    var memStream = new MemoryStreamTiffStream(ms);
    //    using var tiff = Tiff.ClientOpen("in-memory", "r", ms, memStream);
    //}

    [Benchmark]
    public void MagickNet()
    {
        using var image = new MagickImage(Data);
    }

    [Benchmark]
    public void Aspose()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(Data));
    }

    [Benchmark]
    public void ImageSharp()
    {
        using var stream = new MemoryStream(Data);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(stream);
    }
}