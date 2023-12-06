using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using PdfStudy.Benchmark;

//BenchmarkRunner.Run<SaveToJpeg>();
var job = Job.ShortRun;
var config =
    ManualConfig.Create(DefaultConfig.Instance)
        .AddDiagnoser(MemoryDiagnoser.Default)
        .AddJob(job.WithToolchain(
            CsProjCoreToolchain.From(
                new NetCoreAppSettings(
                    targetFrameworkMoniker: "net8.0-windows10.0.19041.0",
                    runtimeFrameworkVersion: null!,
                    name: ".NET 8.0")))
            .WithBaseline(true))
        .AddJob(job.WithRuntime(ClrRuntime.Net481));

//BenchmarkRunner.Run<DecodeWeb>(config);

var switcher = new BenchmarkSwitcher(
    new[]
    {
        typeof(SaveToJpeg),
        typeof(DecodeWeb)
    });

switcher.Run(args, config);