using System.Drawing;
using System.IO;
using ImagingLib.Test.Utility;
using Shouldly;
using SkiaSharp;

namespace ImagingLib.Test;

public class ToBinaryTest : ImageTestBase
{
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    // 現状WebPは非対応
    // [InlineData(ImageFormat.WebP)]
    public void BySystemDrawing(ImageFormat imageFormat)
    {
        using var bitmap = (Bitmap)Image.FromStream(new MemoryStream(LoadBytes(imageFormat)));
        using var actual = bitmap.ToBinary(75);

        var binPath = GetPath($"{imageFormat}.bin", nameof(ToBinaryTest));

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
        using var bitmap = SKBitmap.Decode(LoadBytes(imageFormat));
        using var actual = bitmap.ToBinary(75);

        var binPath = GetPath($"{imageFormat}.bin", nameof(ToBinaryTest));

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
        var imageBytes = LoadBytes(imageFormat);
        using var stream = new MemoryStream(imageBytes);
        using var bitmap = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(stream);
        using var actual = bitmap.ToBinary(75);


        var binPath =
            imageFormat == ImageFormat.Jpeg
                // ImageSharpのJPEGデコーダーはSystem.Drawingとは異るため、結果も変わる
                ? GetPath($"ByImageSharp-{imageFormat}.bin", nameof(ToBinaryTest))
                : GetPath($"{imageFormat}.bin", nameof(ToBinaryTest));

        //File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }
#endif
}