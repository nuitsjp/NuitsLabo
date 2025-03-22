using ImagingTest.Utility;
using System.Drawing;
using ImagingLib;
using Shouldly;
using System.Runtime.InteropServices;

namespace ImagingTest;

public class BinaryTest : ImageTestBase
{
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    // 現状WebPは非対応
    // [InlineData(ImageFormat.WebP)]
    public void SystemDrawing(ImageFormat imageFormat)
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(LoadBytes(imageFormat)));
        using var actual = bitmap.ToBinary(75);

        var binPath = GetPath($"{nameof(SystemDrawing)}-{imageFormat}.bin");

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }
}