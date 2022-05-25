using BenchmarkDotNet.Attributes;

namespace TextTemplateBenchmarks;

public class Benchmarks
{
    [Benchmark]
    public string T4()
    {
        T4TextTemplate template = new("FirstName", "LastName");
        return template.TransformText();
    }
}