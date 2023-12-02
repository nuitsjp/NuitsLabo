using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace WebPBenchmark;

[SimpleJob]
[MemoryDiagnoser]
public class BitmapToBitmapSource : IDisposable
{
    private readonly Bitmap _bitmap = (Bitmap)Image.FromFile("Color.jpg");

    [Benchmark]
    public BitmapSource BitmapData()
    {
        var bitmapData = _bitmap.LockBits(
            new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
            ImageLockMode.ReadOnly, _bitmap.PixelFormat);
        try
        {
            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, 
                bitmapData.Height,
                _bitmap.HorizontalResolution, 
                _bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            return bitmapSource;
        }
        finally
        {
            _bitmap.UnlockBits(bitmapData);
        }
    }

    [Benchmark]
    public BitmapSource SaveJpegStream()
    {
        using var bitmapStream = new MemoryStream();
        _bitmap.Save(bitmapStream, ImageFormat.Jpeg);
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
        using var bitmapStream = new MemoryStream();
        // Bitmapをメモリストリームに保存。デフォルトはJPEG形式。
        // 処理時間とメモリーのバランスを考えると、JPEG形式が最適なため。
        // ただし、府が逆性が重要な場合はPNG形式などを利用するが、ほぼ必要なことはないはず。
        _bitmap.Save(bitmapStream, ImageFormat.Bmp);
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
        var handle = _bitmap.GetHbitmap();
        try
        {
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            return bitmapSource;
        }
        finally { DeleteObject(handle); }
    }


    public void Dispose()
    {
        _bitmap.Dispose();
    }
}