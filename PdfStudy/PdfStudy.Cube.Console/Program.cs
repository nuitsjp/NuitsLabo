using Cube.Pdf.Extensions;
using Cube.Pdf;
using Cube.Pdf.Pdfium;
using System.Drawing.Imaging;

try
{
    var options = new RenderOption()
    {
        Print = true
    };
    using var renderer = new DocumentRenderer("MultiPage.pdf") { RenderOption = options };

    var dpi = 300;
    var pageNo = 0;
    double scale = dpi / 72.0;
    using var bmp = renderer.Render(renderer.GetPage(pageNo + 1), scale);
    using var stream = new MemoryStream();
    bmp.Save(stream, ImageFormat.Jpeg);
    var jpeg = stream.ToArray();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
