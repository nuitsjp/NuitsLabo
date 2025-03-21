using System.Drawing;
using ImagingTest.Utility;
using Shouldly;

namespace ImagingTest;

public class CalculateOtsuThresholdTest : ImageTestBase
{
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void CalculateOtsuThreshold(ImageFormat imageFormat)
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
    public void CalculateOtsuThreshold2(ImageFormat imageFormat)
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(LoadBytes(imageFormat)));
        var actual = bitmap.CalculateOtsuThreshold();

        var expected =
            imageFormat == ImageFormat.Tiff
                ? Threshold.Default
                : (Threshold)69;
        actual.ShouldBe(expected);
    }
}