using AudioCalibrationStudy;

// キャリブレーションポイントを作成
var calibrationPoints = new List<(double Decibels, double Amplitude)>
{
    (50, 0.1),
    (60, 0.3),
    (70, 0.5),
    (78, 0.7),
    (85, 1.0)
};

Console.WriteLine($"Linear\t: {DecibelsToAmplitudeByLinear(75, calibrationPoints)}");
Console.WriteLine($"Spline\t: {DecibelsToAmplitudeBySpline(75, calibrationPoints)}");


static double DecibelsToAmplitudeByLinear(double decibels, List<(double Decibels, double Amplitude)> calibrationPoints)
{
    var calibrator = new AudioLinearCalibrator();
    foreach (var (db, amp) in calibrationPoints)
    {
        calibrator.AddCalibrationPoint(amp, db);
    }

    return calibrator.EstimateAmplitude(decibels);
}

static double DecibelsToAmplitudeBySpline(double decibels, List<(double Decibels, double Amplitude)> calibrationPoints)
{
    var calibrator = new AudioSplineCalibrator(calibrationPoints);
    return calibrator.EstimateAmplitude(decibels);
}