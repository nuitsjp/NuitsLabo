using AudioCalibrationStudy;
using NAudio.Wave;

var calibrator = new AudioCalibrator();

// 測定したポイントを追加（これらの値は実際の測定に基づいて設定する必要があります）
calibrator.AddCalibrationPoint(0.1, 50);  // 振幅0.1で50dB
calibrator.AddCalibrationPoint(0.5, 70);  // 振幅0.5で70dB
calibrator.AddCalibrationPoint(1.0, 85);  // 振幅1.0で85dB

// 目標のデシベル値に対する振幅を推定
double targetDecibels = 60;
double estimatedAmplitude = calibrator.EstimateAmplitude(targetDecibels);

Console.WriteLine($"To achieve {targetDecibels} dB, use amplitude {estimatedAmplitude}");

// NAudioを使用してスピーカー出力を設定
using (var outputDevice = new WaveOutEvent())
{
    // 出力デバイスの音量を設定（0.0から1.0の範囲）
    outputDevice.Volume = (float)estimatedAmplitude;

    // ここで音声を再生
    // ...
}