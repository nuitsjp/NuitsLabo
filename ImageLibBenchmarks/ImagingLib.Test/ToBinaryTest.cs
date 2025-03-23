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
    public void SystemDrawing(ImageFormat imageFormat)
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
    public void SkiaSharp(ImageFormat imageFormat)
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
    public void ImageSharp(ImageFormat imageFormat)
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

    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    // 現状WebPは非対応
    // [InlineData(ImageFormat.WebP)]
    public void SystemDrawingByByteArray(ImageFormat imageFormat)
    {
        using var actual = SystemDrawingExtensions.ToBinary(LoadBytes(imageFormat));

        var binPath = GetPath($"{imageFormat}-ByteArray.bin", nameof(ToBinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }

    [Theory]
    // SkiaSharpはTiffに対応していない
    // [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void SkiaSharpByteArray(ImageFormat imageFormat)
    {
        using var actual = SkiaSharpExtensions.ToBinary(LoadBytes(imageFormat));

        var binPath = GetPath($"{imageFormat}-ByteArray.bin", nameof(ToBinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }

    [Theory]
    // SkiaSharpはTiffに対応していない
    [InlineData(ImageFormat.Tiff)]
    public void LibTiffByteArray(ImageFormat imageFormat)
    {
        using var actual = LibTiffExtensions.ToBinary(LoadBytes(imageFormat));

        var binPath = GetPath($"{imageFormat}-ByteArray.bin", nameof(ToBinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }


    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void MagickNetByteArray(ImageFormat imageFormat)
    {
        using var actual = MagickNetExtensions.ToBinary(LoadBytes(imageFormat));

        var binPath = GetPath($"{imageFormat}-ByteArray.bin", nameof(ToBinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }

#if NET8_0_OR_GREATER
    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    // WebPはAspose.Imagingが非対応
    // [InlineData(ImageFormat.WebP)]
    public void AsposeByByteArray(ImageFormat imageFormat)
    {
        using var actual = AsposeExtensions.ToBinary(LoadBytes(imageFormat));

        var binPath = GetPath($"{imageFormat}-ByteArray.bin", nameof(ToBinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }

    [Theory]
    [InlineData(ImageFormat.Tiff)]
    [InlineData(ImageFormat.Jpeg)]
    [InlineData(ImageFormat.WebP)]
    public void ImageSharpByteArray(ImageFormat imageFormat)
    {
        using var actual = ImageSharpExtensions.ToBinary(LoadBytes(imageFormat));


        var binPath = GetPath($"ByImageSharp-{imageFormat}-ByteArray.bin", nameof(ToBinaryTest));

        // File.WriteAllBytes(binPath, actual.ToBytes());

        actual.ToBytes().ShouldBeEquivalentTo(File.ReadAllBytes(binPath));
    }
#endif
}