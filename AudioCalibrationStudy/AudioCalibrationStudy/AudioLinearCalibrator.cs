namespace AudioCalibrationStudy;

/// <summary>
/// 線形補間を使用してオーディオのキャリブレーションを行うクラス
/// </summary>
internal class AudioLinearCalibrator
{
    /// <summary>
    /// キャリブレーションポイントのリスト
    /// </summary>
    private List<CalibrationPoint> CalibrationPoints { get; } = [];

    /// <summary>
    /// キャリブレーションポイントを追加します
    /// </summary>
    public void AddCalibrationPoint(CalibrationPoint calibrationPoint)
    {
        CalibrationPoints.Add(calibrationPoint);
    }

    /// <summary>
    /// 指定されたデシベル値に対応する振幅を推定します
    /// </summary>
    /// <param name="targetDecibels">目標のデシベル値</param>
    /// <returns>推定された振幅</returns>
    /// <exception cref="ArgumentOutOfRangeException">目標のデシベル値が校正範囲外の場合にスローされます</exception>
    public double EstimateAmplitude(double targetDecibels)
    {
        // 線形補間を使用
        var lowerPoint = CalibrationPoints.LastOrDefault(p => p.Decibels <= targetDecibels);
        var upperPoint = CalibrationPoints.FirstOrDefault(p => p.Decibels > targetDecibels);

        if (lowerPoint == null || upperPoint == null)
        {
            throw new ArgumentOutOfRangeException(nameof(targetDecibels), "目標のデシベル値が校正範囲外です");
        }

        var ratio = (targetDecibels - lowerPoint.Decibels) / (upperPoint.Decibels - lowerPoint.Decibels);
        return lowerPoint.Amplitude + ratio * (upperPoint.Amplitude - lowerPoint.Amplitude);
    }
}