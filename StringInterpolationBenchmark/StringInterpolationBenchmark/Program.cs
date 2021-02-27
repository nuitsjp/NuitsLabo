using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace StringInterpolationBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            new Benchmark().StringBuilder10();
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
