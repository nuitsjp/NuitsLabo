using BenchmarkDotNet.Attributes;

namespace PdfStudy.Benchmark;

public class DecodeWeb
{
    private readonly byte[] _data = File.ReadAllBytes(@"Assets\Color.webp");

    [Benchmark(Baseline = true)]
    public void SystemWindowsMediaImaging()
    {
        PdfStudy.Wpf.WebPDecoder.Decode(_data);
    }

    [Benchmark]
    public async Task WindowsGraphicsImaging()
    {
        await PdfStudy.WindowsDataPdf.WebPDecoder.Decode(_data);
    }
}