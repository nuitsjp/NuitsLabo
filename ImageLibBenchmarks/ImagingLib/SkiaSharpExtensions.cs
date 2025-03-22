using SkiaSharp;

namespace ImagingLib;

public static class SkiaSharpExtensions
{
    /// <summary>
    /// 大津の二値化によるしきい値を求める（SkiaSharp版）。
    /// 画像が既に二値の場合（SKColorType.Index8と仮定）にはデフォルトのしきい値（ここでは50）を返します。
    /// </summary>
    /// <param name="bitmap">処理対象のSKBitmap</param>
    /// <returns>最適なしきい値（0～100の割合）</returns>
    public static unsafe int CalculateOtsu(this SKBitmap bitmap)
    {
        var width = bitmap.Width;
        var height = bitmap.Height;
        var totalPixels = width * height;
        var histogram = new int[256];
        long sumHistogram = 0;

        // グレースケール変換用の係数（各値は元の0.299, 0.587, 0.114を1024倍したもの）
        const int RedFactor = 306;
        const int GreenFactor = 601;
        const int BlueFactor = 117;

        // SKBitmapのピクセルデータにアクセス
        var pixmap = bitmap.PeekPixels();
        if (pixmap == null || pixmap.GetPixels() == IntPtr.Zero)
            throw new InvalidOperationException("ピクセルデータにアクセスできませんでした。");

        // unsafeコードで各ピクセルにアクセス
        var pixels = (byte*)pixmap.GetPixels().ToPointer();
        var rowBytes = pixmap.RowBytes;
        for (var y = 0; y < height; y++)
        {
            var row = pixels + y * rowBytes;
            for (var x = 0; x < width; x++)
            {
                var pixelOffset = x * 4; // 通常は4バイト/ピクセル（Rgba8888またはBgra8888）
                byte r, g, b;
                if (pixmap.ColorType == SKColorType.Rgba8888)
                {
                    r = row[pixelOffset + 0];
                    g = row[pixelOffset + 1];
                    b = row[pixelOffset + 2];
                }
                else if (pixmap.ColorType == SKColorType.Bgra8888)
                {
                    b = row[pixelOffset + 0];
                    g = row[pixelOffset + 1];
                    r = row[pixelOffset + 2];
                }
                else
                {
                    // 万が一他の形式の場合はGetPixelで取得（ただしパフォーマンスは低下します）
                    var color = bitmap.GetPixel(x, y);
                    r = color.Red;
                    g = color.Green;
                    b = color.Blue;
                }

                // グレースケール化（右シフト10で1024で除算）
                var gray = (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
                histogram[gray]++;
            }
        }

        return histogram.OptimalThreshold(width, height);
    }


}