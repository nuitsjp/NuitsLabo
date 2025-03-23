using BenchmarkDotNet.Running;
using Benchmarks.MultiThread;

var summary = BenchmarkRunner.Run<ToBinaryBenchmarks>();