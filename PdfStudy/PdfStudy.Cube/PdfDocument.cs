using System.Drawing.Imaging;
using Cube.Pdf.Pdfium;
using Cube.Pdf.Extensions;

namespace PdfStudy.Cube;


public class PdfDocument : IDisposable
{
    private readonly DocumentRenderer _renderer;

    public PdfDocument(string path)
    {
        var options = new RenderOption()
        {
            Print = true
        };
        _renderer = new DocumentRenderer(path) { RenderOption = options };
    }

    public int PageCount => _renderer.Pages.Count();

    public byte[] ToJpeg(int pageNo, int dpi)
    {
        double scale = dpi / 72.0;
        using var bmp = _renderer.Render(_renderer.GetPage(pageNo + 1), scale);
        using var stream = new MemoryStream();
        bmp.Save(stream, ImageFormat.Jpeg);
        return stream.ToArray();
    }

    public void Dispose()
    {
        _renderer.Dispose();
    }
}