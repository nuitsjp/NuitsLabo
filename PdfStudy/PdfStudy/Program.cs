using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using PdfStudy.Benchmark;

//BenchmarkRunner.Run<SaveToJpeg>();
var job = Job.Default;

var config =
    ManualConfig.Create(DefaultConfig.Instance)
        .AddJob(job.WithToolchain(
            CsProjCoreToolchain.From(
                new NetCoreAppSettings(
                    targetFrameworkMoniker: "net8.0-windows",
                    runtimeFrameworkVersion: null,
                    name: ".NET 8.0"))))
        .AddJob(job.WithRuntime(ClrRuntime.Net481));

BenchmarkRunner.Run<SaveToJpeg>(config);