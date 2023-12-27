using System.Windows;
using System.Windows.Media.Imaging;

namespace ImagingTest.Utility;

public static class BitmapSourceExtensions
{


    public static BitmapSource Crop(this BitmapSource bitmapSource, Rect rect)
    {
        // クロップする領域を定義
        var cropArea = new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);

        // CroppedBitmapを使用して指定領域をクロップ
        var croppedBitmap = new CroppedBitmap(bitmapSource, cropArea);

        return croppedBitmap;
    }

    public static byte[] ToBmpBytes(this BitmapSource bitmapSource)
    {
        // BMPエンコーダのインスタンスを作成
        var encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

        // メモリストリームを使用してBMPデータを書き出し
        using var cropStream = new MemoryStream();
        encoder.Save(cropStream);
        // メモリストリームからバイト配列を取得
        return cropStream.ToArray();
    }

    public static byte[] ToJpegBytes(this BitmapSource bitmapSource)
    {
        // BMPエンコーダのインスタンスを作成
        var encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

        // メモリストリームを使用してBMPデータを書き出し
        using var cropStream = new MemoryStream();
        encoder.Save(cropStream);
        // メモリストリームからバイト配列を取得
        return cropStream.ToArray();
    }
}