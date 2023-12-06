using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using DecodeWebPBenchmarks;
using DecodeWebPBenchmarks.Extensions;


var bitmap = (Bitmap)Image.FromFile("Color.jpg");
var webp = new ImageFormat(new Guid("{b96b3cb7-0728-11d3-9d7b-0000f81ef32e}"));
bitmap.Save("Test.webp", ImageFormat.Webp);

//File.WriteAllBytes("Color.bmp", bitmapSource.ToBmpBytes());
//var job = Job.MediumRun;
//var config =
//    ManualConfig.Create(DefaultConfig.Instance)
//        .AddDiagnoser(MemoryDiagnoser.Default)
//        .AddJob(job.WithToolchain(
//                CsProjCoreToolchain.From(
//                    new NetCoreAppSettings(
//                        targetFrameworkMoniker: "net8.0-windows10.0.19041.0",
//                        runtimeFrameworkVersion: null!,
//                        name: ".NET 8.0")))
//            .WithBaseline(true))
//        .AddJob(job.WithRuntime(ClrRuntime.Net481));

//BenchmarkRunner.Run<DecodeWebP>(config);