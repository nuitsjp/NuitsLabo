using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace Benchmarks;

public static class CustomJobs
{
    public static Job Net80WindowsJob =>
        Job.Default
            .WithRuntime(CoreRuntime.CreateForNewVersion("net8.0-windows", "net8.0-windows"))
            .WithId("net8.0-windows");
}