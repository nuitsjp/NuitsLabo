using BenchmarkDotNet.Running;
using Benchmarks;

BenchmarkSwitcher.FromAssembly(typeof(ToBinarySingleThreadBenchmarks).Assembly).Run(args);