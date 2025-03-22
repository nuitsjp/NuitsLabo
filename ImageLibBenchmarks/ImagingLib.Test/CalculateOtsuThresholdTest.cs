using System.Drawing;
using System.IO;
using ImagingLib.Test.Utility;
using Shouldly;
using SkiaSharp;

namespace ImagingLib.Test;

public class CalculateOtsuThresholdTest : ImageTestBase
{
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    // System.Drawing は WebP に対応していない
    // [InlineData(ImageFormat.WebP)]
    public void SystemDrawing(ImageFormat imageFormat)
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(LoadBytes(imageFormat)));
        var actual = bitmap.CalculateOtsuThreshold();

        var expected = imageFormat == ImageFormat.Tiff ? 75 : 69;
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void SystemWindows(ImageFormat imageFormat)
    {
        var bitmapSource = LoadBytes(imageFormat).ToBitmapSource(imageFormat);
        var actual = bitmapSource.CalculateOtsuThreshold(imageFormat == ImageFormat.Tiff);

        var expected = imageFormat == ImageFormat.Tiff ? 75 : 69;
        actual.ShouldBe(expected);
    }

    [Theory]
    // [InlineData(ImageFormat.Tiff)] // SkiaSharpではTIFF未対応
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void SkiaSharp(ImageFormat imageFormat)
    {
        using var bitmap = SKBitmap.Decode(LoadBytes(imageFormat));
        var actual = bitmap.CalculateOtsu();

        var expected = imageFormat == ImageFormat.Tiff ? 75 : 69;
        actual.ShouldBe(expected);
    }

#if NET8_0_OR_GREATER
    [Theory]
    // [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void CalculateOtsuThresholdByImageSharp(ImageFormat imageFormat)
    {
        var imageBytes = LoadBytes(imageFormat);
        using var stream = new MemoryStream(imageBytes);
        using var imageSharp = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(stream);
        var actual = imageSharp.CalculateOtsu();

        var expected = imageFormat == ImageFormat.Tiff ? 75 : 69;
        actual.ShouldBe(expected);
    }

#endif
}