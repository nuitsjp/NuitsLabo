using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Shouldly;

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
}