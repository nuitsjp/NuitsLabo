using System.IO;
using BenchmarkDotNet.Attributes;

namespace WebPBenchmark;

public abstract class BaseBenchmark
{
    private readonly byte[] _webp = File.ReadAllBytes("Color.webp");
    private readonly byte[] _jpeg = File.ReadAllBytes("Color.jpg");

    [Params("Jpeg", "WebP")]
    public string Format { get; set; } = string.Empty;

    protected bool IsWebP => Format == "WebP";

    protected byte[] Data => IsWebP ? _webp : _jpeg;

}