using Aspose.Pdf;
using Aspose.Pdf.Devices;

namespace PdfStudy.Aspose
{
    public class PdfDocument
    {
        private readonly Document _document;

        public PdfDocument(string path)
        {
            _document = new Document(path);
        }

        public int PageCount => _document.Pages.Count;

        public byte[] ToJpeg(int pageNo, int dpi)
        {
            var page = _document.Pages[pageNo + 1];

            // Create Resolution object
            var resolution = new Resolution(300);

            var jpegDevice = new JpegDevice(resolution);

            // Convert a particular page and save the image to stream
            using var stream = new MemoryStream();
            jpegDevice.Process(page, stream);
            return stream.ToArray();
        }
    }
}