using BenchmarkDotNet.Attributes;

namespace PdfStudy.Benchmark;

public class SaveToJpeg
{
    [Benchmark(Baseline = true)]
    // ReSharper disable once InconsistentNaming
    public void Aspose()
    {
        var document = new PdfStudy.Aspose.PdfDocument(@"Assets\MultiPage.pdf");

        var page0 = document.ToJpeg(0, 300);
        var page1 = document.ToJpeg(1, 300);
    }

    [Benchmark]
    // ReSharper disable once InconsistentNaming
    public void PDFtoImage()
    {
        var document = PdfStudy.PDFtoImage.PdfDocument.Load(@"Assets\MultiPage.pdf");

        var page0 = document.ToJpeg(0, 300);
        var page1 = document.ToJpeg(1, 300);
    }


    [Benchmark]
    // ReSharper disable once InconsistentNaming
    public async Task WindowsDataPdf()
    {
        // ReSharper disable once MethodHasAsyncOverload
        using var stream = new MemoryStream(File.ReadAllBytes(@"Assets\MultiPage.pdf"));
        var document = await PdfStudy.WindowsDataPdf.PdfDocument.LoadAsync(stream);

        var page0 = document.ToJpeg(0, 300);
        var page1 = document.ToJpeg(1, 300);
    }

    [Benchmark]
    // ReSharper disable once InconsistentNaming
    public void Xfinium()
    {
        var document = new PdfStudy.Xfinium.PdfDocument(@"Assets\MultiPage.pdf");

        var page0 = document.ToJpeg(0, 300);
        var page1 = document.ToJpeg(1, 300);
    }
}