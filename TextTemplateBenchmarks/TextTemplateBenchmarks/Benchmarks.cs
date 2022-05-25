using BenchmarkDotNet.Attributes;

namespace TextTemplateBenchmarks;

public class Benchmarks
{
    private readonly RazorBenchmarks _razor = new();
    private readonly ScribanBenchmarks _scriban = new();
    private readonly DotLiquidBenchmarks _dotLiquid = new();
    private readonly HandlebarsBenchmarks _handlebars = new();

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

    [Benchmark]
    public string Scriban() => _scriban.Render();

    [Benchmark]
    public string ScribanWithParse() => _scriban.RenderWithParse();

    [Benchmark]
    public string DotLiquid() => _dotLiquid.Render();

    [Benchmark]
    public string DotLiquidWithParse() => _dotLiquid.RenderWithParse();

    [Benchmark]
    public string Handlebars() => _handlebars.Invoke();

    [Benchmark]
    public string HandlebarsWithCompile() => _handlebars.InvokeWithCompile();
}