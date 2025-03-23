using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ImagingLib;

namespace Benchmarks;

[MemoryDiagnoser]
//[SimpleJob(RuntimeMoniker.Net481, launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1)]
//[SimpleJob(RuntimeMoniker.Net80, launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1)]
[SimpleJob(RuntimeMoniker.Net481)]
[SimpleJob(RuntimeMoniker.Net80)]
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

#if NET8_0_OR_GREATER
    [Benchmark]
    public async Task Aspose()
    {
        var tasks = Enumerable
            .Range(1, 10)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    using var bin = AsposeExtensions.ToBinary(Data);
                }
            }))
            .ToList();
        await Task.WhenAll(tasks);
    }

    [Benchmark]
    public async Task ImageSharp()
    {
        var tasks = Enumerable
            .Range(1, 10)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    using var bin = ImageSharpExtensions.ToBinary(Data);
                }
            }))
            .ToList();
        await Task.WhenAll(tasks);
    }
#endif
}