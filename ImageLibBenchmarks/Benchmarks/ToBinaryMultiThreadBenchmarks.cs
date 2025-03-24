using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ImagingLib;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class ToBinaryMultiThreadBenchmarks : BenchmarkBase
{
    [Benchmark]
    public async Task SystemDrawing()
    {
        var tasks = Enumerable
            .Range(1, 10)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    using var bin = SystemDrawingExtensions.ToBinary(Data);
                }
            }))
            .ToList();
        await Task.WhenAll(tasks);
    }


    [Benchmark]
    public async Task SkiaSharp()
    {
        var tasks = Enumerable
            .Range(1, 10)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    using var bin = SkiaSharpExtensions.ToBinary(Data);
                }
            }))
            .ToList();
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task LibTiff()
    {
        var tasks = Enumerable
            .Range(1, 10)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    using var bin = LibTiffExtensions.ToBinary(Data);
                }
            }))
            .ToList();
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task MagickNet()
    {
        var tasks = Enumerable
            .Range(1, 10)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    using var bin = MagickNetExtensions.ToBinary(Data);
                }
            }))
            .ToList();
        await Task.WhenAll(tasks);
    }
}