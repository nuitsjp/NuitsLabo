using Windows.Graphics.Imaging;

namespace PdfStudy.WindowsDataPdf;

public class WebPDecoder
{
    public static async Task Decode(byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        using var stream = memoryStream.AsRandomAccessStream();
        var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
        await decoder.GetSoftwareBitmapAsync();
    }

}