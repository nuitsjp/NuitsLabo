using System.IO;
using BenchmarkDotNet.Attributes;

namespace WebPBenchmark;

public abstract class BaseBenchmark
{
    private static readonly byte[] WebP = File.ReadAllBytes("Color.webp");
    private static readonly byte[] Jpeg = File.ReadAllBytes("Color.jpg");
    private static readonly byte[] Tiff = File.ReadAllBytes("Color.tiff");

    [Params("Jpeg", "WebP", "Tiff")]
    public string Format { get; set; } = string.Empty;

    protected bool IsWebP => Format == "WebP";

    protected byte[] Data =>
        Format switch
        {
            "Jpeg" => Jpeg,
            "WebP" => WebP,
            "Tiff" => Tiff,
            _ => throw new NotSupportedException()
        };
}