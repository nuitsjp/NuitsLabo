using System.IO;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;
using ImageMagick;
using ImagingLib;

namespace Benchmarks.SingleThread;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 1, invocationCount: 1)]
// [SimpleJob]
public class ToBinaryBenchmarks : BenchmarkBase
{
    private readonly byte[] _data = File.ReadAllBytes("Color.jpg");

    /// <summary>
    /// 2値化しきい値
    /// </summary>
    private static readonly float Threshold = 0.75f;

    //[Benchmark]
    //public BitmapSource MagickNetFixedThreshold()
    //{
    //    using var magickImage = new MagickImage(BitmapSource.ToBmpBytes());

    //    magickImage.Threshold(new Percentage(Threshold));
    //    magickImage.Depth = 1;

    //    // MagickImageからMemoryStreamに変換
    //    using var stream = new MemoryStream();
    //    magickImage.Write(stream, MagickFormat.Bmp);  // 一時的にBMP形式として出力
    //    stream.Position = 0;

    //    var bitmapImage = new BitmapImage();
    //    bitmapImage.BeginInit();
    //    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
    //    bitmapImage.StreamSource = stream;
    //    bitmapImage.EndInit();
    //    bitmapImage.Freeze();

    //    return bitmapImage;
    //}

    [Benchmark]
    public void SystemDrawing()
    {
        using var bin = SystemDrawingExtensions.ToBinary(Data);
    }


    [Benchmark]
    public void SkiaSharp()
    {
        using var bin = SkiaSharpExtensions.ToBinary(Data);
    }



    [Benchmark]
    public void LibTiff()
    {
        using var bin = LibTiffExtensions.ToBinary(Data);
    }

#if NET8_0_OR_GREATER
    [Benchmark]
    public void Aspose()
    {
        using var bin = AsposeExtensions.ToBinary(Data);
    }

    [Benchmark]
    public void ImageSharp()
    {
        using var bin = ImageSharpExtensions.ToBinary(Data);
    }
#endif
}