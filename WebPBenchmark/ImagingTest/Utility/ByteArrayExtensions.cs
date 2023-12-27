using System.Windows.Media.Imaging;

namespace ImagingTest.Utility;

public static class ByteArrayExtensions
{
    public static BitmapSource ToBitmapSource(this byte[] image, ImageFormat imageFormat)
    {
        using var stream = new MemoryStream(image);

        // MemoryStreamからBitmapImageに変換
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();  // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        return bitmapImage;
        //// WebP形式以外の場合は、そのまま返す
        //if (imageFormat != ImageFormat.WebP)
        //{
        //}

        //var bitmap = bitmapImage.ToBitmap(image.DpiX, image.DpiY);
        //return bitmap.ToBitmapSource();
    }
}