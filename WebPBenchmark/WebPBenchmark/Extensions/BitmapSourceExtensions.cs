using System.IO;
using System.Windows.Media.Imaging;

namespace WebPBenchmark.Extensions;

public static class BitmapSourceExtensions
{
    /// <summary>
    /// BitmapSourceをBMP形式のバイト配列に変換
    /// </summary>
    /// <param name="bitmapSource"></param>
    /// <returns></returns>
    public static byte[] ToBmpBytes(this BitmapSource bitmapSource)
    {
        using var bitmapMemory = new MemoryStream();
        var encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        encoder.Save(bitmapMemory);
        return bitmapMemory.ToArray();
    }
}