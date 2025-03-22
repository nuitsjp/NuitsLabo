using BenchmarkDotNet.Running;
using Benchmarks.SingleThread;

var summary = BenchmarkRunner.Run<ToBinaryBenchmarks>();