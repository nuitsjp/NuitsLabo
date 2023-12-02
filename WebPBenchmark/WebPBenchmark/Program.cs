using System.IO;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebPBenchmark;

var summary = BenchmarkRunner.Run<BitmapSourceToBitmap>();
//BenchmarkRunner.Run(typeof(Load).Assembly, DefaultConfig.Instance);