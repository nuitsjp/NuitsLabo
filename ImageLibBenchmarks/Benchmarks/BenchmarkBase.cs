using System.IO;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class BenchmarkBase
{
    private static readonly byte[] WebP = File.ReadAllBytes("Color.webp");
    private static readonly byte[] Jpeg = File.ReadAllBytes("Color.jpg");
    private static readonly byte[] Tiff = File.ReadAllBytes("Color.tiff");

    [Params("Jpeg", "WebP", "Tiff")]
    public virtual string Format { get; set; } = "WebP";

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