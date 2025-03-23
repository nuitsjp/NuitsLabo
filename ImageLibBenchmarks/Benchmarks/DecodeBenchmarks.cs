using System.Drawing;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BitMiracle.LibTiff.Classic;
using ImageMagick;
using ImagingLib;
using SkiaSharp;

namespace Benchmarks;

[MemoryDiagnoser]
//[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1)]
[SimpleJob(RuntimeMoniker.Net481)]
[SimpleJob(RuntimeMoniker.Net80)]
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

    [Benchmark]
    public void LibTiff()
    {
        using var ms = new MemoryStream(Data);
        var memStream = new MemoryStreamTiffStream(ms);
        using var tiff = Tiff.ClientOpen("in-memory", "r", ms, memStream);
    }

    [Benchmark]
    public void MagickNet()
    {
        using var image = new MagickImage(Data);
    }

#if NET8_0_OR_GREATER
    [Benchmark]
    public void Aspose()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(Data));
    }

    [Benchmark]
    public void ImageSharp()
    {
        using var stream = new MemoryStream(Data);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);
    }
#endif
}