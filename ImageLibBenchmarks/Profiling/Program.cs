using System.IO;
using ImagingLib;

var imageBytes = File.ReadAllBytes("Color.jpg");

for (var i = 0; i < 5; i++)
{
    using var bin = SkiaSharpExtensions.ToBinary(imageBytes);
}