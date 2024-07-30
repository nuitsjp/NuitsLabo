namespace AudioCalibrationStudy;

internal class AudioLinearCalibrator
{
    // 測定したサンプルポイントを格納する構造体
    struct CalibrationPoint
    {
        public double Amplitude;
        public double Decibels;
    }

    private List<CalibrationPoint> CalibrationPoints { get; } = [];

    // 測定したポイントを追加
    public void AddCalibrationPoint(double amplitude, double decibels)
    {
        CalibrationPoints.Add(new CalibrationPoint { Amplitude = amplitude, Decibels = decibels });
    }

    // 振幅からデシベルを推定

    // デシベルから必要な振幅を推定
    public double EstimateAmplitude(double targetDecibels)
    {
        // 同様に、線形補間を使用
        var lowerPoint = CalibrationPoints.LastOrDefault(p => p.Decibels <= targetDecibels);
        var upperPoint = CalibrationPoints.FirstOrDefault(p => p.Decibels > targetDecibels);

        if (lowerPoint.Equals(default(CalibrationPoint)) || upperPoint.Equals(default(CalibrationPoint)))
        {
            throw new ArgumentOutOfRangeException(nameof(targetDecibels));
        }

        var ratio = (targetDecibels - lowerPoint.Decibels) / (upperPoint.Decibels - lowerPoint.Decibels);
        return lowerPoint.Amplitude + ratio * (upperPoint.Amplitude - lowerPoint.Amplitude);
    }
}