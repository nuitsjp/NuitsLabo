using BenchmarkDotNet.Running;
using Benchmarks;
using Benchmarks.Net80;

BenchmarkSwitcher.FromAssembly(typeof(ToBinarySingleThreadBenchmarks).Assembly).Run(args);