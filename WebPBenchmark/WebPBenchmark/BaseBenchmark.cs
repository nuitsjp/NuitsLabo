using System.IO;
using BenchmarkDotNet.Attributes;

namespace WebPBenchmark;

public abstract class BaseBenchmark
{
    private readonly byte[] _webp = File.ReadAllBytes("Color.webp");
    private readonly byte[] _jpeg = File.ReadAllBytes("Color.jpg");
    private readonly byte[] _tiff = File.ReadAllBytes("Color.tiff");

    [Params("Jpeg", "WebP", "Tiff")]
    public string Format { get; set; } = string.Empty;

    protected bool IsWebP => Format == "WebP";

    protected byte[] Data =>
        Format switch
        {
            "Jpeg" => _jpeg,
            "WebP" => _webp,
            "Tiff" => _tiff,
            _ => throw new NotSupportedException()
        };
}