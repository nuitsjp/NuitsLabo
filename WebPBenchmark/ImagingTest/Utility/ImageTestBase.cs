using System.Runtime.CompilerServices;

namespace ImagingTest.Utility;

public abstract class ImageTestBase
{
    protected static byte[] Tiff => File.ReadAllBytes(@"ImageTestBase\Binary.tiff");
    protected static byte[] Jpeg => File.ReadAllBytes(@"ImageTestBase\Color.jpg");
    protected static byte[] WebP => File.ReadAllBytes(@"ImageTestBase\Color.webp");

    protected static byte[] LoadBytes(ImageFormat format)
    {
        return format switch
        {
            ImageFormat.Tiff => Tiff,
            ImageFormat.Jpeg => Jpeg,
            ImageFormat.WebP => WebP,
            _ => throw new ArgumentException("Invalid image format", nameof(format))
        };
    }

    protected static string GetPath(
        string fileName,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "")
    {
        var fileInfo = new FileInfo(filePath);
        var directory = fileInfo.Directory!;
        if (directory.Exists is false)
        {
            directory.Create();
        }
        return Path.Combine(directory.FullName, memberName, fileName);
    }
}


/// <summary>
/// 画像フォーマットID
/// </summary>
public enum ImageFormat : byte
{
    /// <summary>
    /// TIFF
    /// </summary>
    Tiff = 0,
    /// <summary>
    /// JPEG
    /// </summary>
    Jpeg = 1,
    /// <summary>
    /// WebP
    /// </summary>
    WebP = 2

}
