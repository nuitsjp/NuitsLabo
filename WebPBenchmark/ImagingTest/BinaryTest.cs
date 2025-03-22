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
    public void BySystemDrawing(ImageFormat imageFormat)
    {
        using var actual = LoadBytes(imageFormat).BySystemDrawing(75);

        var binPath = GetPath($"{imageFormat}.bin", nameof(BinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }

    [Theory]
    // SkiaSharpはTiffに対応していない
    // [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void BySkiaSharp(ImageFormat imageFormat)
    {
        using var actual = LoadBytes(imageFormat).BySkiaSharp(75);

        var binPath = GetPath($"{imageFormat}.bin", nameof(BinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }

#if NET8_0_OR_GREATER
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void ByImageSharp(ImageFormat imageFormat)
    {
        using var actual = LoadBytes(imageFormat).ByImageSharp(75);


        var binPath = 
            imageFormat == ImageFormat.Jpeg
                // ImageSharpのJPEGデコーダーはSystem.Drawingとは異るため、結果も変わる
                ? GetPath($"ByImageSharp-{imageFormat}.bin", nameof(BinaryTest))
                : GetPath($"{imageFormat}.bin", nameof(BinaryTest));

        //File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }
#endif
}