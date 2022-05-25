using BenchmarkDotNet.Attributes;

namespace TextTemplateBenchmarks;

public class Benchmarks
{
    private readonly RazorBenchmarks _razor = new RazorBenchmarks();

    [Benchmark]
    public string T4()
    {
        T4TextTemplate template = new("FirstName", "LastName");
        return template.TransformText();
    }

    [Benchmark]
    public string Razor() => _razor.Run();

    [Benchmark]
    public string RazorWithCompile() => _razor.RunWithCompile();
}