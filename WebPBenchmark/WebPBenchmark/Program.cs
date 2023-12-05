using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using Sharprompt;
using WebPBenchmark;

var summary = BenchmarkRunner.Run<Crop>();

//var jobs = new[] { Job.ShortRun, Job.Default };
//var jobName = Prompt.Select("Job", jobs.Select(x => x.ToString()));

//var job = jobs.Single(x => x.ToString() == jobName);

//var config =
//    job == Job.ShortRun
//        ? ManualConfig.Create(DefaultConfig.Instance)
//            .AddJob(job.WithRuntime(ClrRuntime.Net481))
//        : ManualConfig.Create(DefaultConfig.Instance)
//            .AddJob(job.WithToolchain(
//                CsProjCoreToolchain.From(
//                    new NetCoreAppSettings(
//                        targetFrameworkMoniker: "net8.0-windows",
//                        runtimeFrameworkVersion: null,
//                        name: ".NET 8.0"))))
//            .AddJob(job.WithRuntime(ClrRuntime.Net481));

//var switcher = new BenchmarkSwitcher(
//    new[]
//    {
//        typeof(BinarizeByFixedThreshold), 
//        typeof(BinarizeByOtsu),
//        typeof(BitmapSourceToBitmap),
//        typeof(BitmapToBitmapSource),
//        typeof(CreateThumbnail),
//        typeof(Crop),
//        typeof(Load),
//    });

//switcher.Run(args, config);
