using ImageMagick;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using FluentAssertions;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace ImagingTest
{
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

        [Theory]
        [InlineData(ImageFormat.Tiff)]
        [InlineData(ImageFormat.Jpeg)]
        [InlineData(ImageFormat.WebP)]
        public void SystemWindowsAndImageSharp(ImageFormat imageFormat)
        {
            var imageBytes = LoadBytes(imageFormat);

            Compare(
                FromSystemWindows(imageBytes, imageFormat),
                FromImageSharp(imageBytes));
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
                FromImageSharp(imageBytes));
        }

        private bool CompareBitmaps(BitmapSource bitmapSource, Image<Rgba32> imageSharp)
        {
            if (bitmapSource.PixelWidth != imageSharp.Width || bitmapSource.PixelHeight != imageSharp.Height)
                return false;

            // ピクセルデータのコピー
            var pixelsWpf = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * 4];
            bitmapSource.CopyPixels(pixelsWpf, bitmapSource.PixelWidth * 4, 0);

            for (var y = 0; y < bitmapSource.PixelHeight; y++)
            {
                for (var x = 0; x < bitmapSource.PixelWidth; x++)
                {
                    var index = (y * bitmapSource.PixelWidth + x) * 4;
                    var pixelWpf = new Rgba32(pixelsWpf[index + 2], pixelsWpf[index + 1], pixelsWpf[index], pixelsWpf[index + 3]); // BGRA to RGBA

                    var pixelImageSharp = imageSharp[x, y];

                    if (!pixelWpf.Equals(pixelImageSharp))
                        return false;
                }
            }

            return true;
        }

        private bool CompareBitmaps(Bitmap bitmap, BitmapSource bitmapSource)
        {
            if (bitmap.Width != bitmapSource.PixelWidth || bitmap.Height != bitmapSource.PixelHeight)
                return false;

            var pixelsWpf = new byte[bitmapSource.PixelHeight * bitmapSource.PixelWidth * 4];
            bitmapSource.CopyPixels(pixelsWpf, bitmapSource.PixelWidth * 4, 0);

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixelDrawing = bitmap.GetPixel(x, y);
                    var index = (y * bitmapSource.PixelWidth + x) * 4;
                    var pixelWpf = System.Drawing.Color.FromArgb(pixelsWpf[index + 3], pixelsWpf[index + 2], pixelsWpf[index + 1], pixelsWpf[index]);

                    if (pixelDrawing != pixelWpf)
                        return false;
                }
            }

            return true;
        }

        private void Compare(IEnumerable<System.Drawing.Color> colors1, IEnumerable<System.Drawing.Color> colors2)
        {
            var list1 = colors1.ToList();
            var list2 = colors2.ToList();

            list1.Count.Should().Be(list2.Count);

            for (var i = 0; i < list1.Count; i++)
            {
                var color1 = list1[i];
                var color2 = list2[i];

                color1.A.Should().Be(color2.A);
                color1.R.Should().Be(color2.R);
                color1.G.Should().Be(color2.G);
                color1.B.Should().Be(color2.B);
            }
        }

        private IEnumerable<Color> FromDrawing(byte[] imageBytes)
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

        private IEnumerable<Color> FromMagickDotNet(byte[] imageBytes)
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

        private IEnumerable<Color> FromImageSharp(byte[] imageBytes)
        {
            using var stream = new MemoryStream(imageBytes);
            using var imageSharp = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);

            for (var y = 0; y < imageSharp.Height; y++)
            {
                for (var x = 0; x < imageSharp.Width; x++)
                {
                    var pixel2 = imageSharp[x, y];

                    yield return Color.FromArgb(pixel2.A, pixel2.R, pixel2.G, pixel2.B);
                }
            }
        }

        private IEnumerable<Color> FromSystemWindows(byte[] imageBytes, ImageFormat imageFormat)
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
                    yield return System.Drawing.Color.FromArgb(
                        pixels[index + 3],
                        pixels[index + 2],
                        pixels[index + 1],
                        pixels[index]);
                }
            }
        }
    }
}