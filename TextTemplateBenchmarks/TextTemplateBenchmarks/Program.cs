using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using TextTemplateBenchmarks;

var summary = BenchmarkRunner
    .Run<Benchmarks>(
        ManualConfig
            .Create(DefaultConfig.Instance)
            .WithOptions(ConfigOptions.DisableOptimizationsValidator));