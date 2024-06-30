using NAudio.Wave;
using NAudioStudy;

using var waveIn = new WaveInEvent();
waveIn.WaveFormat = new WaveFormat(44100, 1);
var sampleProvider = new WaveInProvider(waveIn).ToSampleProvider();
var aWeightingFilter = new AWeightingFilter(sampleProvider);

waveIn.DataAvailable += (s, e) =>
{
    float[] buffer = new float[e.BytesRecorded / 2];
    int samplesRead = aWeightingFilter.Read(buffer, 0, buffer.Length);

    // 音量計算（RMS値）
    double sum = 0;
    for (int i = 0; i < samplesRead; i++)
    {
        sum += buffer[i] * buffer[i];
    }
    double rms = Math.Sqrt(sum / samplesRead);
    double db = 20 * Math.Log10(rms);

    Console.WriteLine($"A-weighted Volume: {db} dB");
};

waveIn.StartRecording();
Console.WriteLine("Press any key to stop...");
Console.ReadKey();
waveIn.StopRecording();