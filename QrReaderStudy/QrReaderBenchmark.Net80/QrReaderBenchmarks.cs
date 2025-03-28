using Aspose.BarCode.BarCodeRecognition;
using Aspose.Drawing;
using BenchmarkDotNet.Attributes;
using BitMiracle.LibTiff.Classic;
using ZXing;
using ZXing.Net.Bindings.LibTiff;

namespace QrReaderBenchmark.Net80;

[MemoryDiagnoser]
[SimpleJob]
public class QrReaderBenchmarks
{
    private static readonly byte[] QrCodeBytes = File.ReadAllBytes("SinglePage.tif");

    static QrReaderBenchmarks()
    {
        using var stream = File.Open("Aspose.BarCode.NET.lic", FileMode.Open);
        new License().SetLicense(stream);
    }

    [Benchmark]
    public Result[] ZXing()
    {
        using var stream = File.Open("MultiPage.tif", FileMode.Open);
        using var tiff = Tiff.ClientOpen("in-memory", "r", stream, new TiffStream());

        var reader = new BarcodeReader();
        return reader.DecodeMultiple(tiff);
    }

    [Benchmark]
    public BarCodeResult[] Aspose_HighPerformance()
    {
        using var stream = new MemoryStream(QrCodeBytes);
        using var bitmap = new Bitmap(stream);
        using var reader = new BarCodeReader(bitmap);
        reader.Timeout = (int)TimeSpan.FromSeconds(10).TotalMicroseconds;
        reader.QualitySettings = QualitySettings.HighPerformance;
        return reader.ReadBarCodes();
    }

    [Benchmark]
    public BarCodeResult[]? Aspose_NormalQuality()
    {
        using var stream = new MemoryStream(QrCodeBytes);
        using var bitmap = new Bitmap(stream);
        using var reader = new BarCodeReader(bitmap);
        reader.Timeout = (int)TimeSpan.FromSeconds(10).TotalMicroseconds;
        reader.QualitySettings = QualitySettings.NormalQuality;
        return reader.ReadBarCodes();
    }

    [Benchmark]
    public BarCodeResult[]? Aspose_HighQuality()
    {
        using var stream = new MemoryStream(QrCodeBytes);
        using var bitmap = new Bitmap(stream);
        using var reader = new BarCodeReader(bitmap);
        reader.Timeout = (int)TimeSpan.FromSeconds(10).TotalMicroseconds;
        reader.QualitySettings = QualitySettings.HighQuality;
        return reader.ReadBarCodes();
    }

    [Benchmark]
    public BarCodeResult[]? Aspose_MaxQuality()
    {
        using var stream = new MemoryStream(QrCodeBytes);
        using var bitmap = new Bitmap(stream);
        using var reader = new BarCodeReader(bitmap);
        reader.Timeout = (int)TimeSpan.FromSeconds(10).TotalMicroseconds;
        reader.QualitySettings = QualitySettings.MaxQuality;
        return reader.ReadBarCodes();
    }
}