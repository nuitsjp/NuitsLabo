using System;
using BenchmarkDotNet.Running;

namespace T4TemplateBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //new SourceGeneratorBenchmarks().CustomT4Template01();
            //var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            //BenchmarkRunner.Run<ToStringBenchmarks>();
            BenchmarkRunner.Run<SourceGeneratorBenchmarks>();
        }
    }
}
