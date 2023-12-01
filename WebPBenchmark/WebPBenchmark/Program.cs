using BenchmarkDotNet.Running;
using WebPBenchmark;

var summary = BenchmarkRunner.Run<CreateThumbnail>();