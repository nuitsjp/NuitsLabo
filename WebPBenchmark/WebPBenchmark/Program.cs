using BenchmarkDotNet.Running;
using WebPBenchmark;

//new BitmapToBitmapSource { Format = "Jpeg" }.BitmapData();
var summary = BenchmarkRunner.Run<BitmapToBitmapSource>();
//BenchmarkRunner.Run(typeof(Load).Assembly, DefaultConfig.Instance);