using System.Drawing;
using System.Drawing.Imaging;
using Xfinium.Pdf;
using Xfinium.Pdf.Rendering;

namespace PdfStudy.Xfinium
{
    public class PdfDocument
    {
        private readonly PdfFixedDocument _document;

        public PdfDocument(string path)
        {
            _document = new PdfFixedDocument(path);
        }

        public int PageCount => _document.Pages.Count;

        public byte[] ToJpeg(int pageNo, int dpi)
        {
            var page = _document.Pages[pageNo];

            // レンダリングオプションの設定
            var settings = new PdfRendererSettings
            {
                DpiX = dpi,
                DpiY = dpi
            };

            // ページを画像としてレンダリング
            var renderer = new PdfPageRenderer(page);
            using var stream = new MemoryStream();
            renderer.ConvertPageToImage(stream, PdfPageImageFormat.Png, settings);

            // PNGからJPEGに変換
            using var image = (Bitmap)Image.FromStream(stream);
            using var jpegStream = new MemoryStream();
            image.SetResolution(dpi, dpi);
            image.Save(jpegStream, ImageFormat.Jpeg);
            return jpegStream.ToArray();
        }
    }
}