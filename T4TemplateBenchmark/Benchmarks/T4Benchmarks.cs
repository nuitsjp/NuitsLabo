using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net471)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    public class T4Benchmarks
    {
        private string _namespace;
        private string _name;
        [GlobalSetup]
        public void Benchmark()
        {
            _namespace = Guid.NewGuid().ToString();
            _name = Guid.NewGuid().ToString();
        }

        [Benchmark]
        public string Default()
        {
            return new Default
            {
                Namespace = _namespace,
                Name = _name
            }.TransformText();
        }

        [Benchmark]
        public string CustomWithObject()
        {
            return new CustomWithObject()
            {
                Namespace = _namespace,
                Name = _name
            }.TransformText();
        }

        [Benchmark]
        public string CustomWithString()
        {
            return new CustomWithString()
            {
                Namespace = _namespace,
                Name = _name
            }.TransformText();
        }


        [Benchmark]
        public string StringInterpolation()
        {
            return new StringInterpolation()
            {
                Namespace = _namespace,
                Name = _name
            }.TransformText();
        }


        [Benchmark]
        public string StringBuilderBench()
        {
            return new StringBuilderBench()
            {
                Namespace = _namespace,
                Name = _name
            }.TransformText();
        }
    }
}