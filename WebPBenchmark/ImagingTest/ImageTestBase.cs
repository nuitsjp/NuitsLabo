using System.Runtime.CompilerServices;

namespace ImagingTest;

public abstract class ImageTestBase
{
    protected byte[] Tiff => File.ReadAllBytes(@"ImageTestBase\Binary.tiff");
    protected byte[] Jpeg => File.ReadAllBytes(@"ImageTestBase\Color.jpg");
    protected byte[] WebP => File.ReadAllBytes(@"ImageTestBase\Color.webp");

    protected byte[] LoadBytes(ImageFormat format)
    {
        return format switch
        {
            ImageFormat.Tiff => Tiff,
            ImageFormat.Jpeg => Jpeg,
            ImageFormat.WebP => WebP,
            _ => throw new ArgumentException()
        };
    }

    protected static string GetPath(
        string fileName,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "")
    {
        var fileInfo = new FileInfo(filePath);
        var directory = fileInfo.Directory!;
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