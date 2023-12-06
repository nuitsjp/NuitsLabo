using FluentAssertions;
using PdfStudy.PDFtoImage;

namespace PdfStudy.Test
{
    public class SaveToJpegTest
    {
        [Fact]
        public void Aspose()
        {
            var document = new Aspose.PdfDocument(@"Assets\MultiPage.pdf");
            document.PageCount.Should().Be(2);

            var page0 = document.ToJpeg(0, 300);
            //File.WriteAllBytes(@"Assets\Aspose_Page0.jpg", page0);
            page0.Should().BeEquivalentTo(File.ReadAllBytes(@"Assets\Aspose_Page0.jpg"));

            var page1 = document.ToJpeg(1, 300);
            //File.WriteAllBytes(@"Assets\Aspose_Page1.jpg", page1);
            page1.Should().BeEquivalentTo(File.ReadAllBytes(@"Assets\Aspose_Page1.jpg"));
        }

        [Fact]
        public void PdfToImage()
        {
            var document = PdfDocument.Load(@"Assets\MultiPage.pdf");
            document.PageCount.Should().Be(2);

            var page0 = document.ToJpeg(0, 300);
            //File.WriteAllBytes(@"Assets\PdfToImage_Page0.jpg", page0);
            page0.Should().BeEquivalentTo(File.ReadAllBytes(@"Assets\PdfToImage_Page0.jpg"));

            var page1 = document.ToJpeg(1, 300);
            //File.WriteAllBytes(@"Assets\PdfToImage_Page1.jpg", page1);
            page1.Should().BeEquivalentTo(File.ReadAllBytes(@"Assets\PdfToImage_Page1.jpg"));
        }

        [Fact]
        public void Xfinium()
        {
            var document = new Xfinium.PdfDocument(@"Assets\MultiPage.pdf");
            document.PageCount.Should().Be(2);

            var page0 = document.ToJpeg(0, 300);
            //File.WriteAllBytes(@"Assets\Xfinium_Page0.jpg", page0);
            page0.Should().BeEquivalentTo(File.ReadAllBytes(@"Assets\Xfinium_Page0.jpg"));

            var page1 = document.ToJpeg(1, 300);
            //File.WriteAllBytes(@"Assets\Xfinium_Page1.jpg", page1);
            page1.Should().BeEquivalentTo(File.ReadAllBytes(@"Assets\Xfinium_Page1.jpg"));
        }

    }
}