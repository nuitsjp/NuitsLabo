using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
    /// <param name="bitmapSource"></param>
    /// <param name="dpiX"></param>
    /// <param name="dpiY"></param>
    /// <returns></returns>
    public static Bitmap ToBitmap(this BitmapSource bitmapSource, int? dpiX = null, int? dpiY = null)
    {
        // BitmapSourceからピクセルデータを取得
        var width = bitmapSource.PixelWidth;
        var height = bitmapSource.PixelHeight;

        // Bitmapを作成
        var bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
        // DPIを設定
        bitmap.SetResolution(dpiX ?? (int)bitmapSource.DpiX, dpiY ?? (int)bitmapSource.DpiY);
        // Bitmapをロック
        var data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
        try
        {
            var stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8); // 1行あたりのバイト数
            var bufferSize = stride * bitmapSource.PixelHeight;
            // BitmapSourceかBitmapへピクセルデータをコピー
            bitmapSource.CopyPixels(Int32Rect.Empty, data.Scan0, bufferSize, stride);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }

        return bitmap;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static BitmapSource ToBitmapSource(this Bitmap bitmap, ImageFormat? format = null)
    {
        using var bitmapStream = new MemoryStream();
        // Bitmapをメモリストリームに保存。デフォルトはJPEG形式。
        // 処理時間とメモリーのバランスを考えると、JPEG形式が最適なため。
        // ただし、府が逆性が重要な場合はPNG形式などを利用するが、ほぼ必要なことはないはず。
        bitmap.Save(bitmapStream, format ?? ImageFormat.Jpeg);
        bitmapStream.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = bitmapStream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
    }

    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject([In] IntPtr hObject);
}