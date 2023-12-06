using BenchmarkDotNet.Attributes;

namespace PdfStudy.Benchmark
{
    [SimpleJob]
    [MemoryDiagnoser]
    public class SaveToJpeg
    {
        [Benchmark]
        // ReSharper disable once InconsistentNaming
        public void Aspose()
        {
            var document = new PdfStudy.Aspose.PdfDocument(@"Assets\MultiPage.pdf");

            var page0 = document.ToJpeg(0, 300);
            var page1 = document.ToJpeg(1, 300);
        }

        [Benchmark]
        // ReSharper disable once InconsistentNaming
        public void PDFtoImage(){
            var document = PdfStudy.PDFtoImage.PdfDocument.Load(@"Assets\MultiPage.pdf");

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
}