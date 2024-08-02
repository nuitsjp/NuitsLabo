namespace AudioCalibrationStudy;

/// <summary>
/// キャリブレーションポイントを表すレコード
/// </summary>
/// <param name="Decibels">デシベル値</param>
/// <param name="Amplitude">振幅</param>
public record CalibrationPoint(double Decibels, double Amplitude);