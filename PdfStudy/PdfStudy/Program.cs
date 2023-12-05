using System.Drawing;
using System.Drawing.Imaging;

var pdfBytes = File.ReadAllBytes("FromPowerPoint.pdf");
using var pdfStream = new MemoryStream(pdfBytes);
var pageCount = PDFtoImage.Conversion.GetPageCount(pdfBytes);

for (int i = 0; i < pageCount; i++)
{
    using var stream = new MemoryStream();
    PDFtoImage.Conversion.SaveJpeg(stream, pdfBytes, page: i, dpi: 300);
    var image = (Bitmap)Image.FromStream(stream);
    image.SetResolution(300, 300);
    image.Save($"Page{i}.jpg", ImageFormat.Jpeg);
}
