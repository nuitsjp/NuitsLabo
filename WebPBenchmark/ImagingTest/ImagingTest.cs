using ImageMagick;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;
using ImagingTest.Utility;
using Shouldly;
#if NET8_0_OR_GREATER
using SixLabors.ImageSharp.PixelFormats;
#endif

namespace ImagingTest;

public class ImagingTest : ImageTestBase
{
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    // WebPは未対応
    // [InlineData(ImageFormat.WebP)]
    public void DrawingAndSystemWindows(ImageFormat imageFormat)
    {
        var imageBytes = LoadBytes(imageFormat);
        Compare(
            FromDrawing(imageBytes),
            FromSystemWindows(imageBytes, imageFormat));
    }

    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void SystemWindowsAndMagickDotNet(ImageFormat imageFormat)
    {
        var imageBytes = LoadBytes(imageFormat);

        Compare(
            FromSystemWindows(imageBytes, imageFormat),
            FromMagickDotNet(imageBytes));
    }

#if NET8_0_OR_GREATER
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void SystemWindowsAndImageSharp(ImageFormat imageFormat)
    {
        var imageBytes = LoadBytes(imageFormat);

        Compare(
            FromSystemWindows(imageBytes, imageFormat),
            FromImageSharp(imageBytes),
            imageFormat == ImageFormat.Jpeg
                ? 64 // ImageSharpのJPEGデコーダは色が少し異なる
                : 0);
    }

    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void MagickDotNetAndImageSharp(ImageFormat imageFormat)
    {
        var imageBytes = LoadBytes(imageFormat);

        Compare(
            FromMagickDotNet(imageBytes),
            FromImageSharp(imageBytes),
            imageFormat == ImageFormat.Jpeg
                ? 64 // ImageSharpのJPEGデコーダは色が少し異なる
                : 0);
    }
#endif

    private static void Compare(IEnumerable<Color> colors1, IEnumerable<Color> colors2, double tolerance = 0)
    {
        var list1 = colors1.ToList();
        var list2 = colors2.ToList();

        list1.Count.ShouldBe(list2.Count);

        for (var i = 0; i < list1.Count; i++)
        {
            var color1 = list1[i];
            var color2 = list2[i];

            color1.A.ToShort().ShouldBe(color2.A, tolerance);
            color1.R.ToShort().ShouldBe(color2.R, tolerance);
            color1.G.ToShort().ShouldBe(color2.G, tolerance);
            color1.B.ToShort().ShouldBe(color2.B, tolerance);
        }
    }

    private static IEnumerable<Color> FromDrawing(byte[] imageBytes)
    {
        using var stream = new MemoryStream(imageBytes);
        using var bitmap = (Bitmap)System.Drawing.Image.FromStream(stream);
        for (var y = 0; y < bitmap.Height; y++)
        {
            for (var x = 0; x < bitmap.Width; x++)
                yield return bitmap.GetPixel(x, y);
        }
        bitmap.Dispose();

    }

    private static IEnumerable<Color> FromMagickDotNet(byte[] imageBytes)
    {
        using var magickImage = new MagickImage(imageBytes);

        using var pixels = magickImage.GetPixels();
        for (var y = 0; y < magickImage.Height; y++)
        {
            for (var x = 0; x < magickImage.Width; x++)
            {
                var pixel = pixels[x, y]!.ToColor()!;
                yield return Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
            }
        }
    }

#if NET8_0_OR_GREATER
    private static IEnumerable<Color> FromImageSharp(byte[] imageBytes)
    {
        using var stream = new MemoryStream(imageBytes);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);

        for (var y = 0; y < imageSharp.Height; y++)
        {
            for (var x = 0; x < imageSharp.Width; x++)
            {
                var pixel = imageSharp[x, y];

                yield return Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
            }
        }
    }
#endif

    private static IEnumerable<Color> FromSystemWindows(byte[] imageBytes, ImageFormat imageFormat)
    {
        BitmapSource bitmapSource;
        if (imageFormat == ImageFormat.Tiff)
        {
            using var stream = new MemoryStream(imageBytes);
            var source = new BitmapImage();
            source.BeginInit();
            source.CacheOption = BitmapCacheOption.OnLoad;
            source.StreamSource = stream;
            source.EndInit();
            source.Freeze();

            var convertedBitmapSource = new FormatConvertedBitmap();
            convertedBitmapSource.BeginInit();
            convertedBitmapSource.Source = source;
            convertedBitmapSource.DestinationFormat = PixelFormats.Bgra32;
            convertedBitmapSource.EndInit();
            convertedBitmapSource.Freeze();
            bitmapSource = convertedBitmapSource;
        }
        else
        {
            using var stream = new MemoryStream(imageBytes);
            var source = new BitmapImage();
            source.BeginInit();
            source.CacheOption = BitmapCacheOption.OnLoad;
            source.StreamSource = stream;
            source.EndInit();
            source.Freeze();

            bitmapSource = source;
        }

        var pixels = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * 4];
        bitmapSource.CopyPixels(pixels, bitmapSource.PixelWidth * 4, 0);

        for (var y = 0; y < bitmapSource.PixelHeight; y++)
        {
            for (var x = 0; x < bitmapSource.PixelWidth; x++)
            {
                var index = (y * bitmapSource.PixelWidth + x) * 4;
                yield return Color.FromArgb(
                    pixels[index + 3],
                    pixels[index + 2],
                    pixels[index + 1],
                    pixels[index]);
            }
        }
    }
}

public static class ByteExtensions
{
    public static float ToShort(this byte value)
    {
        return value;
    }
}