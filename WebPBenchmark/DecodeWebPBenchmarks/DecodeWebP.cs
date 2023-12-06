using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BenchmarkDotNet.Attributes;
using Windows.Graphics.Imaging;

namespace DecodeWebPBenchmarks;

public class DecodeWebP
{
    private readonly byte[] _data = File.ReadAllBytes("Color.webp");

    [Benchmark(Baseline = true)]
    public BitmapSource SystemWindowsMediaImaging()
    {
        using var bitmapMemory = new MemoryStream(_data);

        // MemoryStreamからBitmapImageに変換
        var sourceImage = new BitmapImage();
        sourceImage.BeginInit();
        sourceImage.CacheOption = BitmapCacheOption.OnLoad;
        sourceImage.StreamSource = bitmapMemory;
        sourceImage.EndInit();
        sourceImage.Freeze(); // これはUIスレッド外でBitmapSourceを安全に使用するための重要なステップです

        return sourceImage;
    }


    [Benchmark]
    public async Task<BitmapSource> WindowsGraphicsImaging()
    {
        using var memoryStream = new MemoryStream(_data);
        using var stream = memoryStream.AsRandomAccessStream();
        var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
        var softwareBitmap = await decoder.GetSoftwareBitmapAsync();

        // SoftwareBitmap を BGRA8 と Premultiplied Alpha に変換
        using var convertedBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        var bytes = new byte[convertedBitmap.PixelHeight * convertedBitmap.PixelWidth * 4];
        convertedBitmap.CopyToBuffer(bytes.AsBuffer());

        // bytes を BitmapSource に変換
        var dpiX = 72.0;
        var dpiY = 72.0;
        var pixelFormat = PixelFormats.Bgra32;
        var stride = convertedBitmap.PixelWidth * ((pixelFormat.BitsPerPixel + 7) / 8);

        var bitmapSource = BitmapSource.Create(convertedBitmap.PixelWidth, convertedBitmap.PixelHeight,
            dpiX, dpiY, pixelFormat, null, bytes, stride);

        return bitmapSource;
    }

    [Benchmark]
    public async Task<SoftwareBitmap> WindowsGraphicsImagingDecodeOnly()
    {
        using var memoryStream = new MemoryStream(_data);
        using var stream = memoryStream.AsRandomAccessStream();
        var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
        return await decoder.GetSoftwareBitmapAsync();
    }
}