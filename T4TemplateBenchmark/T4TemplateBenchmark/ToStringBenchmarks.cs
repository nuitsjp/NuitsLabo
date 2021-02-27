using BenchmarkDotNet.Attributes;

namespace T4TemplateBenchmark
{
    public class ToStringBenchmarks
    {
        [Params(10000, 100000)] public int N;

        private string _value;
        [Benchmark]
        public void FromObject()
        {
            for (int i = 0; i < N; i++)
            {
                _value = ToStringFromObject("MyNamespace");
            }
        }

        [Benchmark]
        public void FromString()
        {
            for (int i = 0; i < N; i++)
            {
                _value = ToStringFromString("MyNamespace");
            }
        }

        private string ToStringFromObject(object value) => (string) value;
        private string ToStringFromString(string value) => value;
    }
}