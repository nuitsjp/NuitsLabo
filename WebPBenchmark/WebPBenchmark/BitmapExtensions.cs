using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WebPBenchmark;

public static class BitmapExtensions
{
    /// <summary>
    /// BitmapSourceをBitmapに変換
    /// </summary>
    /// <param name="bitmapImage"></param>
    /// <param name="dpiX"></param>
    /// <param name="dpiY"></param>
    /// <returns></returns>
    public static Bitmap ToBitmap(this BitmapImage bitmapImage, int? dpiX = null, int? dpiY = null)
    {
        // BitmapSourceからピクセルデータを取得
        var width = bitmapImage.PixelWidth;
        var height = bitmapImage.PixelHeight;

        // Bitmapを作成
        var bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
        // DPIを設定
        bitmap.SetResolution(dpiX ?? (int)bitmapImage.DpiX, dpiY ?? (int)bitmapImage.DpiY);
        // Bitmapをロック
        var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
        try
        {
            var stride = width * ((bitmapImage.Format.BitsPerPixel + 7) / 8); // 1行あたりのバイト数
            var bufferSize = stride * bitmapImage.PixelHeight;
            // BitmapSourceかBitmapへピクセルデータをコピー
            bitmapImage.CopyPixels(Int32Rect.Empty, data.Scan0, bufferSize, stride);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }

        return bitmap;
    }


    public static BitmapSource ToBitmapSource(this Bitmap bitmap)
    {
        var handle = bitmap.GetHbitmap();
        try
        {
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            return bitmapSource;
        }
        finally { DeleteObject(handle); }
    }

    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject([In] IntPtr hObject);
}