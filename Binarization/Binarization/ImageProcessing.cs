using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Binarization;

public class ImageProcessing
{
    public static void BinarizeByOtsu(string inputPath, string outputPath)
    {
        // 画像ファイルを読み込む
        using var image = CvInvoke.Imread(inputPath, ImreadModes.Grayscale);
        using var thresholdImage = new Mat();

        if (image.IsEmpty)
        {
            throw new Exception("画像を読み込めませんでした。");
        }

        // 大津の二値化を適用
        CvInvoke.Threshold(image, thresholdImage, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);

        // TIFFとして保存
        CvInvoke.Imwrite(outputPath, thresholdImage, new KeyValuePair<ImwriteFlags, int>(ImwriteFlags.TiffCompression, 4 /*CompressionCCITTFAX4*/));
    }
}