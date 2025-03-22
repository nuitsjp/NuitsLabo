using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using System.Windows.Media.Imaging;
using ImageMagick;
using Rectangle = System.Drawing.Rectangle;
using WebPBenchmark.Extensions;
using BenchmarkDotNet.Jobs;
using ImagingLib;

namespace WebPBenchmark;

[MemoryDiagnoser]
[SimpleJob]
public class BinarizeByOtsu : BaseBenchmark
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    [Benchmark]
    public BitmapSource MagickNet()
    {
        using var magickImage = new MagickImage(BitmapSource.ToBmpBytes());

        // 画像をグレースケールに変換
        magickImage.ColorSpace = ColorSpace.Gray;
        // 大津の二値化アルゴリズムを適用して二値化する
        magickImage.AutoThreshold(AutoThresholdMethod.OTSU);

        // MagickImageからMemoryStreamに変換
        using var stream = new MemoryStream();
        magickImage.Write(stream, MagickFormat.Bmp);  // 一時的にBMP形式として出力
        stream.Position = 0;

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();

        return bitmapImage;
    }

//    [Benchmark]
//    public void BySystemDrawing()
//    {
//        using var bin = _data.(Threshold);
//    }


//    [Benchmark]
//    public void BySkiaSharp()
//    {
//        using var bin = _data.BySkiaSharp(Threshold);
//    }

//#if NET8_0_OR_GREATER
//    [Benchmark]
//    public void ByImageSharp()
//    {
//        using var bin = _data.ByImageSharp(Threshold);
//    }
//#endif

}
