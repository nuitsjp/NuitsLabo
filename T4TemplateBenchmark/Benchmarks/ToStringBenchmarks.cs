using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net471)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class ToStringBenchmarks
    {
        private string _input;
        private object _inputObject;

        [GlobalSetup]
        public void Setup()
        {
            _input = Guid.NewGuid().ToString();
            _inputObject = _input;
        }

        [Benchmark]
        public string ObjectToString() => (string)_inputObject;

        [Benchmark]
        public string StringToString() => _input;
    }
}