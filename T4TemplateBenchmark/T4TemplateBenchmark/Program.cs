using System;
using BenchmarkDotNet.Running;

namespace T4TemplateBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
