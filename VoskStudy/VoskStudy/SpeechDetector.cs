using NAudio.Wave;
using Vosk;
using System;
using System.Threading.Tasks;

public class SpeechDetector : IDisposable
{
    private readonly Model model;
    private VoskRecognizer recognizer;
    private WaveInEvent waveIn;
    private bool isListening = false;

    public event EventHandler<string> KeywordDetected;

    public SpeechDetector(string modelPath)
    {
        var fullPath = new DirectoryInfo(modelPath).FullName;
        model = new Model(fullPath);
        recognizer = new VoskRecognizer(model, 16000.0f);
        InitializeAudio();
    }

    private void InitializeAudio()
    {
        waveIn = new WaveInEvent
        {
            WaveFormat = new WaveFormat(16000, 1)
        };
        waveIn.DataAvailable += WaveIn_DataAvailable;
    }

    public void StartListening()
    {
        if (!isListening)
        {
            waveIn.StartRecording();
            isListening = true;
        }
    }

    public void StopListening()
    {
        if (isListening)
        {
            waveIn.StopRecording();
            isListening = false;
        }
    }

    private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        var startTime = DateTime.Now;
        if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
        {
            var result = recognizer.Result();
            Console.WriteLine($"認識結果: {result} 経過時間:{DateTime.Now - startTime}");
            ProcessResult(result);
        }
    }

    private void ProcessResult(string result)
    {
        // 結果から単語を抽出（簡略化のため、文字列操作を使用）
        if (result.Contains("スタート") || result.Contains("ストップ"))
        {
            KeywordDetected?.Invoke(this, result.Contains("スタート") ? "スタート" : "ストップ");
        }
    }

    public void Dispose()
    {
        StopListening();
        waveIn?.Dispose();
        recognizer?.Dispose();
        model?.Dispose();
    }
}

