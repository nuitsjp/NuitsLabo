using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using PDFtoImage;

namespace PdfStudy.PDFtoImage
{
    public class PdfDocument
    {
        private readonly byte[] _pdfBytes;

        public PdfDocument(byte[] pdfBytes)
        {
            _pdfBytes = pdfBytes;
        }

        public int PageCount => Conversion.GetPageCount(_pdfBytes);

        public byte[] ToJpeg(int page, int dpi)
        {
            using var bitmapStream = new MemoryStream();
            Conversion.SaveJpeg(bitmapStream, _pdfBytes, page: page, dpi: dpi);
            using var image = (Bitmap)Image.FromStream(bitmapStream);
            image.SetResolution(dpi, dpi);

            using var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public static PdfDocument Load(string path)
        {
            var pdfBytes = File.ReadAllBytes(path);
            return new PdfDocument(pdfBytes);
        }
    }
}