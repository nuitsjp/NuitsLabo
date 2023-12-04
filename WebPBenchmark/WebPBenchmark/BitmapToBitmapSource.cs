using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace WebPBenchmark;

[ShortRunJob]
[MemoryDiagnoser]
public class BitmapToBitmapSource : BaseBenchmark
{
    [Benchmark]
    public BitmapSource BitmapData()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(Data));
        var bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly, bitmap.PixelFormat);
        try
        {
            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, 
                bitmapData.Height,
                bitmap.HorizontalResolution, 
                bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            return bitmapSource;
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }
    }

    [Benchmark]
    public BitmapSource SaveJpegStream()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(Data));
        using var bitmapStream = new MemoryStream();
        bitmap.Save(bitmapStream, ImageFormat.Jpeg);
        bitmapStream.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = bitmapStream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
    }

    [Benchmark]
    public BitmapSource SaveBmpStream()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(Data));
        using var bitmapStream = new MemoryStream();
        // Bitmapをメモリストリームに保存。デフォルトはJPEG形式。
        // 処理時間とメモリーのバランスを考えると、JPEG形式が最適なため。
        // ただし、府が逆性が重要な場合はPNG形式などを利用するが、ほぼ必要なことはないはず。
        bitmap.Save(bitmapStream, ImageFormat.Bmp);
        bitmapStream.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = bitmapStream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
    }

    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    public static extern bool DeleteObject(System.IntPtr hObject);

    [Benchmark]
    public BitmapSource CreateBitmapSourceFromHBitmap()
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(Data));
        var handle = bitmap.GetHbitmap();
        try
        {
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            return bitmapSource;
        }
        finally { DeleteObject(handle); }
    }
}