#if NET8_0_OR_GREATER
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace ImagingLib;

public static class ImageSharpExtensions
{
    public const int DefaultThreshold = 75;

    /// <summary>
    /// 二値化時に利用する赤色の重み
    /// </summary>
    private const int RedFactor = (int)(0.298912 * 1024);

    /// <summary>
    /// 二値化時に利用する緑色の重み
    /// </summary>
    private const int GreenFactor = (int)(0.586611 * 1024);

    /// <summary>
    /// 二値化時に利用する青色の重み
    /// </summary>
    private const int BlueFactor = (int)(0.114478 * 1024);

    /// <summary>
    /// Otsu の二値化により最適なしきい値を求める (百分率で返す)。
    /// ImageSharp は 1bpp をサポートしないため、そのチェックは行っていません。
    /// </summary>
    /// <param name="image">対象の Image&lt;Rgba32&gt; 画像</param>
    /// <returns>最適なしきい値 (Threshold 型にキャスト)</returns>
    public static int CalculateOtsu(this Image<Rgba32> image)
    {
        var width = image.Width;
        var height = image.Height;
        var totalPixels = width * height;
        var histogram = new int[256];

        // 画像全体を走査し、グレイスケール値に変換しながらヒストグラムを作成
        image.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < height; y++)
            {
                var pixelRow = accessor.GetRowSpan(y);
                for (var x = 0; x < width; x++)
                {
                    var pixel = pixelRow[x];
                    // (R * 306 + G * 601 + B * 117) >> 10 でグレイスケール値を算出
                    var gray = (pixel.R * RedFactor + pixel.G * GreenFactor + pixel.B * BlueFactor) >> 10;
                    histogram[gray]++;
                }
            }
        });

        return histogram.OptimalThreshold(width, height);
    }
}
#endif