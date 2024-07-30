namespace AudioCalibrationStudy;

class AudioCalibrator
{
    // 測定したサンプルポイントを格納する構造体
    struct CalibrationPoint
    {
        public double Amplitude;
        public double Decibels;
    }

    private List<CalibrationPoint> calibrationPoints;

    public AudioCalibrator()
    {
        calibrationPoints = new List<CalibrationPoint>();
    }

    // 測定したポイントを追加
    public void AddCalibrationPoint(double amplitude, double decibels)
    {
        calibrationPoints.Add(new CalibrationPoint { Amplitude = amplitude, Decibels = decibels });
    }

    // 振幅からデシベルを推定
    public double EstimateDecibels(double amplitude)
    {
        // 簡単な線形補間を使用。より精密な補間方法も考えられます。
        var lowerPoint = calibrationPoints.LastOrDefault(p => p.Amplitude <= amplitude);
        var upperPoint = calibrationPoints.FirstOrDefault(p => p.Amplitude > amplitude);

        if (lowerPoint.Equals(default(CalibrationPoint)) || upperPoint.Equals(default(CalibrationPoint)))
        {
            throw new ArgumentOutOfRangeException("Amplitude is out of calibrated range");
        }

        double ratio = (amplitude - lowerPoint.Amplitude) / (upperPoint.Amplitude - lowerPoint.Amplitude);
        return lowerPoint.Decibels + ratio * (upperPoint.Decibels - lowerPoint.Decibels);
    }

    // デシベルから必要な振幅を推定
    public double EstimateAmplitude(double targetDecibels)
    {
        // 同様に、線形補間を使用
        var lowerPoint = calibrationPoints.LastOrDefault(p => p.Decibels <= targetDecibels);
        var upperPoint = calibrationPoints.FirstOrDefault(p => p.Decibels > targetDecibels);

        if (lowerPoint.Equals(default(CalibrationPoint)) || upperPoint.Equals(default(CalibrationPoint)))
        {
            throw new ArgumentOutOfRangeException("Target decibels is out of calibrated range");
        }

        double ratio = (targetDecibels - lowerPoint.Decibels) / (upperPoint.Decibels - lowerPoint.Decibels);
        return lowerPoint.Amplitude + ratio * (upperPoint.Amplitude - lowerPoint.Amplitude);
    }
}