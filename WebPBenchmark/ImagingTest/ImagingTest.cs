using ImageMagick;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using FluentAssertions;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Windows.Media;

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
            using var bitmap = FromDrawing(imageBytes);
            var bitmapSource = FromSystemWindows(imageBytes);

            // BitmapSourceを32ビットカラーに変換
            var convertedBitmapSource = new FormatConvertedBitmap();
            convertedBitmapSource.BeginInit();
            convertedBitmapSource.Source = bitmapSource;
            convertedBitmapSource.DestinationFormat = PixelFormats.Bgra32;
            convertedBitmapSource.EndInit();
            
            var pixelsWpf = new byte[convertedBitmapSource.PixelHeight * convertedBitmapSource.PixelWidth * 4];
            convertedBitmapSource.CopyPixels(pixelsWpf, convertedBitmapSource.PixelWidth * 4, 0);

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixelDrawing = bitmap.GetPixel(x, y);
                    var index = (y * convertedBitmapSource.PixelWidth + x) * 4;
                    var pixelWpf = System.Drawing.Color.FromArgb(pixelsWpf[index + 3], pixelsWpf[index + 2], pixelsWpf[index + 1], pixelsWpf[index]);
                    (pixelDrawing == pixelWpf).Should().BeTrue();
                }
            }
        }


        [Theory]
        [InlineData(ImageFormat.Tiff)]
        [InlineData(ImageFormat.Jpeg)]
        // WebPは未対応
        // [InlineData(ImageFormat.WebP)]
        public void DrawingAndAndMagickDotNet(ImageFormat imageFormat)
        {
            var imageBytes = LoadBytes(imageFormat);
            using var fromDrawing = FromDrawing(imageBytes);
            using var fromMagickDotNet = FromMagickDotNet(imageBytes);

            CompareBitmaps(fromDrawing, fromMagickDotNet).Should().BeTrue();
        }

        [Theory]
        [InlineData(ImageFormat.Tiff)]
        [InlineData(ImageFormat.Jpeg)]
        // WebPは未対応
        // [InlineData(ImageFormat.WebP)]
        public void DrawingAndAndImageSharp(ImageFormat imageFormat)
        {
            var imageBytes = LoadBytes(imageFormat);
            using var fromDrawing = FromDrawing(imageBytes);
            using var fromImageSharp = FromImageSharp(imageBytes);

            CompareBitmaps(fromDrawing, fromImageSharp).Should().BeTrue();
        }

        [Theory]
        [InlineData(ImageFormat.Tiff)]
        [InlineData(ImageFormat.Jpeg)]
        [InlineData(ImageFormat.WebP)]
        public void MagickDotNetAndImageSharp(ImageFormat imageFormat)
        {
            var imageBytes = LoadBytes(imageFormat);
            using var fromMagickDotNet = FromMagickDotNet(imageBytes);
            using var fromImageSharp = FromImageSharp(imageBytes);

            CompareBitmaps(fromMagickDotNet, fromImageSharp).Should().BeTrue();
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

        private bool CompareBitmaps(MagickImage magickImage, Image<Rgba32> imageSharp)
        {
            if (magickImage.Width != imageSharp.Width || magickImage.Height != imageSharp.Height)
                return false;

            using var pixels = magickImage.GetPixels();
            for (var y = 0; y < magickImage.Height; y++)
            {
                for (var x = 0; x < magickImage.Width; x++)
                {
                    var pixel1 = pixels[x, y].ToColor();
                    var pixel2 = imageSharp[x, y];

                    // Magick.NETのカラー値を8ビットの範囲に変換
                    var color1 = System.Drawing.Color.FromArgb(pixel2.A, pixel2.R, pixel2.G, pixel2.B);

                    if (color1.R != pixel2.R || color1.G != pixel2.G || color1.B != pixel2.B || color1.A != pixel2.A)
                        return false;
                }
            }

            return true;
        }

        private bool CompareBitmaps(Bitmap bitmap, Image<Rgba32> imageSharp)
        {
            if (bitmap.Width != imageSharp.Width || bitmap.Height != imageSharp.Height)
                return false;

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel1 = bitmap.GetPixel(x, y);
                    var pixel2 = imageSharp[x, y];

                    if (pixel1.R != pixel2.R || pixel1.G != pixel2.G || pixel1.B != pixel2.B || pixel1.A != pixel2.A)
                        return false;
                }
            }

            return true;
        }

        private Bitmap FromDrawing(byte[] imageBytes)
        {
            using var stream = new MemoryStream(imageBytes);
            return (Bitmap)System.Drawing.Image.FromStream(stream);
        }

        private MagickImage FromMagickDotNet(byte[] imageBytes)
        {
            return new MagickImage(imageBytes);
        }

        private SixLabors.ImageSharp.Image<Rgba32> FromImageSharp(byte[] imageBytes)
        {
            using var stream = new MemoryStream(imageBytes);
            return SixLabors.ImageSharp.Image.Load<Rgba32>(stream);
        }

        private BitmapSource FromSystemWindows(byte[] imageBytes)
        {
            using var stream = new MemoryStream(imageBytes);
            var source = new BitmapImage();
            source.BeginInit();
            source.CacheOption = BitmapCacheOption.OnLoad;
            source.StreamSource = stream;
            source.EndInit();
            source.Freeze();
            return source;
        }

        private bool CompareBitmaps(Bitmap bitmap1, MagickImage magickImage2)
        {
            {
                if (bitmap1.Width != magickImage2.Width || bitmap1.Height != magickImage2.Height)
                    return false;

                using (var pixels = magickImage2.GetPixels())
                {
                    for (var y = 0; y < bitmap1.Height; y++)
                    {
                        for (var x = 0; x < bitmap1.Width; x++)
                        {
                            var pixel1 = bitmap1.GetPixel(x, y);
                            var pixel2 = pixels[x, y].ToColor();

                            var color2 = System.Drawing.Color.FromArgb(pixel2.A, pixel2.R, pixel2.G, pixel2.B);

                            if (pixel1.ToArgb() != color2.ToArgb())
                                return false;
                        }
                    }
                }

                return true;
            }
        }
    }
}