using System;
using BenchmarkDotNet.Running;
using Benchmarks;

namespace T4TemplateBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //new SourceGeneratorBenchmarks().CustomT4Template01();
            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            BenchmarkRunner.Run<T4Benchmarks>();
            //BenchmarkRunner.Run<SourceGeneratorBenchmarks>();
        }
    }
}
