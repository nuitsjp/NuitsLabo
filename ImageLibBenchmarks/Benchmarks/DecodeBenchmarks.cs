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
}