using System.IO;
using System.Windows.Media.Imaging;

namespace PdfStudy.Wpf;

public class WebPDecoder
{
    public static void Decode(byte[] data)
    {
        using var bitmapMemory = new MemoryStream(data);

        // MemoryStreamからBitmapImageに変換
        var sourceImage = new BitmapImage();
        sourceImage.BeginInit();
        sourceImage.CacheOption = BitmapCacheOption.OnLoad;
        sourceImage.StreamSource = bitmapMemory;
        sourceImage.EndInit();
        sourceImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです
    }

}