using System;
using BenchmarkDotNet.Running;

namespace PLinqStudyBench
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<PLinqTest>();
        }
    }
}