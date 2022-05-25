using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using T4vsRazor;

var summary = BenchmarkRunner
    .Run<T4vsRazorBench>(
        ManualConfig
            .Create(DefaultConfig.Instance)
            .WithOptions(ConfigOptions.DisableOptimizationsValidator));