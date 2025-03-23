using System.Drawing;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
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

#if NET8_0_OR_GREATER
    [Benchmark]
    public void ImageSharp()
    {
        using var stream = new MemoryStream(Data);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(stream);
    }
#endif
}