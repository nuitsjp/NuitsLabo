using BenchmarkDotNet.Running;
using Benchmarks.Decode;

var summary = BenchmarkRunner.Run<DecodeBenchmarks>();