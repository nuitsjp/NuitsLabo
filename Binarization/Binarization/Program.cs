using System.Drawing;
using Idw.Image;
using Sharprompt;

var imageFile =
    new FileInfo(
        args.Any()
            ? args[0]
            : Prompt.Input<string>("Image file:"));

using var bitmap = (Bitmap)Image.FromFile(imageFile.FullName);
using var binBitmap = bitmap.ToBinary(bitmap.CalculateOtsuThreshold());

var binFileName = imageFile.Name.Substring(0, imageFile.Name.Length - imageFile.Extension.Length) + "-bin.tif";
File.WriteAllBytes(
    Path.Combine(imageFile.DirectoryName!, binFileName),
    binBitmap.ToTiffBytes());