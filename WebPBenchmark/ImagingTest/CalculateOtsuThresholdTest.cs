using System.Drawing;
using ImagingTest.Utility;
using Shouldly;
using SkiaSharp;

#if NET8_0_OR_GREATER
using SixLabors.ImageSharp.PixelFormats;
#endif

namespace ImagingTest;

public class CalculateOtsuThresholdTest : ImageTestBase
{
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void CalculateOtsuThresholdByBitmapSource(ImageFormat imageFormat)
    {
        var bitmapSource = LoadBytes(imageFormat).ToBitmapSource(imageFormat);
        var actual = bitmapSource.CalculateOtsuThreshold(imageFormat);

        var expected =
            imageFormat == ImageFormat.Tiff
                ? Threshold.Default
                : (Threshold)69;
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    // 現状WebPは非対応
    // [InlineData(ImageFormat.WebP)]
    public void CalculateOtsuThresholdByBitmap(ImageFormat imageFormat)
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(LoadBytes(imageFormat)));
        var actual = bitmap.CalculateOtsuThreshold();

        var expected =
            imageFormat == ImageFormat.Tiff
                ? Threshold.Default
                : (Threshold)69;
        actual.ShouldBe(expected);
    }

    [Theory]
    // [InlineData(ImageFormat.Tiff)] // SkiaSharpではTIFF未対応
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void CalculateOtsuThresholdBySkiaSharp(ImageFormat imageFormat)
    {
        using var bitmap = SKBitmap.Decode(LoadBytes(imageFormat));
        var actual = bitmap.CalculateOtsuThreshold();

        var expected =
            imageFormat == ImageFormat.Tiff
                ? Threshold.Default
                : (Threshold)69;
        actual.ShouldBe(expected);
    }

#if NET8_0_OR_GREATER
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void CalculateOtsuThresholdByImageSharp(ImageFormat imageFormat)
    {
        var imageBytes = LoadBytes(imageFormat);
        using var stream = new MemoryStream(imageBytes);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);
        var actual = imageSharp.CalculateOtsuThreshold();

        var expected =
            imageFormat == ImageFormat.Tiff
                ? Threshold.Default
                : (Threshold)69;
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void GetHistogram(ImageFormat imageFormat)
    {
        var imageBytes = LoadBytes(imageFormat);
        using var stream = new MemoryStream(imageBytes);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);
        var bitmapSource = imageBytes.ToBitmapSource(imageFormat);

        var expected = bitmapSource.GetHistogram(imageFormat);
        var actual = imageSharp.GetHistogram();

        actual.ShouldBeEquivalentTo(expected);
    }
#endif
}