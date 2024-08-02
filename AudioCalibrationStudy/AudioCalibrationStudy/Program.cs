using AudioCalibrationStudy;

// キャリブレーションポイントを作成
var calibrationPoints = new List<CalibrationPoint>
{
    new(Amplitude: 10, Decibels: 50),
    new(Amplitude: 30, Decibels: 60),
    new(Amplitude: 50, Decibels: 70),
    new(Amplitude: 70, Decibels: 78),
    new(Amplitude: 100, Decibels: 85)
};

Console.WriteLine($"Linear\t: {DecibelsToAmplitudeByLinear(75, calibrationPoints)}");
Console.WriteLine($"Spline\t: {DecibelsToAmplitudeBySpline(75, calibrationPoints)}");

static double DecibelsToAmplitudeByLinear(double decibels, List<CalibrationPoint> calibrationPoints)
{
    var calibrator = new AudioLinearCalibrator();
    foreach (var point in calibrationPoints)
    {
        calibrator.AddCalibrationPoint(point);
    }
    return calibrator.EstimateAmplitude(decibels);
}

static double DecibelsToAmplitudeBySpline(double decibels, List<CalibrationPoint> calibrationPoints)
{
    var calibrator = new AudioSplineCalibrator(calibrationPoints);
    return calibrator.EstimateAmplitude(decibels);
}