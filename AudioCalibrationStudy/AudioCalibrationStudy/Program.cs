using AudioCalibrationStudy;

// キャリブレーションポイントを作成
var calibrationPoints = new List<(double Decibels, double Amplitude)>
{
    (50, 10),
    (60, 30),
    (70, 50),
    (78, 70),
    (85, 100)
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