using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using T4vsRazor;

var summary = BenchmarkRunner
    .Run<T4VsRazorBench>(
        ManualConfig
            .Create(DefaultConfig.Instance)
            .WithOptions(ConfigOptions.DisableOptimizationsValidator));