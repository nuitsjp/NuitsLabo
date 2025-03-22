namespace ImagingLib;

public static class BaseExtensions
{
    public static int OptimalThreshold(this int[] histogram, int width, int height)
    {
        // 画像全体のピクセル数
        var totalPixels = width * height;
        float sumOfHistogram = 0;

        // ヒストグラムの合計を計算
        for (var i = 0; i < 256; i++)
        {
            sumOfHistogram += i * histogram[i];
        }

        float sumB = 0; // 背景のピクセルの合計
        var weightBackground = 0; // 背景の重み

        float maxVariance = 0; // 最大分散
        var optimalThreshold = 0; // 最適なしきい値

        // 各階調に対して最適なしきい値を計算
        for (var i = 0; i < 256; i++)
        {
            weightBackground += histogram[i];
            if (weightBackground == 0) continue;

            var weightForeground = totalPixels - weightBackground; // 前景の重み
            if (weightForeground == 0) break;

            sumB += i * histogram[i];
            var meanBackground = sumB / weightBackground;
            var meanForeground = (sumOfHistogram - sumB) / weightForeground;

            // 背景と前景の分散を計算
            var varianceBetween = weightBackground * (float)weightForeground * (meanBackground - meanForeground) * (meanBackground - meanForeground);
            if (varianceBetween > maxVariance)
            {
                maxVariance = varianceBetween;
                optimalThreshold = i;
            }
        }

        return (int)(optimalThreshold / 256f * 100);
    }
}