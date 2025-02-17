using BenchmarkDotNet.Running;
using FixedLengthFileStudy.Benchmark;

var summary = BenchmarkRunner.Run<ByteStreamReaderBenchmark>();