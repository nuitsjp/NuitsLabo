using System.IO;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace WebPBenchmark;

public abstract class BaseBenchmark
{
    private static readonly byte[] WebP = File.ReadAllBytes("Color.webp");
    private static readonly byte[] Jpeg = File.ReadAllBytes("Color.jpg");
    private static readonly byte[] Tiff = File.ReadAllBytes("Color.tiff");

    private static readonly BitmapSource WebPBitmapSource = CreateBitmapSource(WebP);
    private static readonly BitmapSource JpegBitmapSource = CreateBitmapSource(Jpeg);
    private static readonly BitmapSource TiffBitmapSource = CreateBitmapSource(Tiff);

    [Params("Jpeg", "WebP", "Tiff")]
    public string Format { get; set; } = "WebP";

    protected bool IsWebP => Format == "WebP";

    protected byte[] Data =>
        Format switch
        {
            "Jpeg" => Jpeg,
            "WebP" => WebP,
            "Tiff" => Tiff,
            _ => throw new NotSupportedException()
        };

    protected BitmapSource BitmapSource =>
        Format switch
        {
            "Jpeg" => JpegBitmapSource,
            "WebP" => WebPBitmapSource,
            "Tiff" => TiffBitmapSource,
            _ => throw new NotSupportedException()
        };

    /// <summary>
    /// バイト列からBitmapSourceを生成します。
    /// </summary>
    private static BitmapSource CreateBitmapSource(byte[] data)
    {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = new MemoryStream(data);
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
    }
    
}