using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Text.RegularExpressions;

namespace MicrophoneVolumeLogger;

public class Microphone : IDisposable
{
    private readonly MMDevice _device;

    private readonly WasapiCapture _capture;

    private Timer? _timer;

    private readonly List<float> _masterPeakValues = new();

    public Microphone(MMDevice device)
    {
        _device = device;
        _capture = new WasapiCapture(device);
    }

    public string FriendlyName => _device.FriendlyName;

    public float MasterVolumeLevelScalar
    {
        get => _device.AudioEndpointVolume.MasterVolumeLevelScalar;
        set => _device.AudioEndpointVolume.MasterVolumeLevelScalar = value;
    }
    public float MasterPeakValue => _device.AudioMeterInformation.MasterPeakValue;

    public IReadOnlyList<float> MasterPeakValues => _masterPeakValues;

    public void StartRecoding()
    {
        _masterPeakValues.Clear();
        _capture.StartRecording();
        _timer = new Timer(state =>
        {
            _masterPeakValues.Add(_device.AudioMeterInformation.MasterPeakValue);
        }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(50));
    }

    public void StopRecording()
    {
        _timer?.Dispose();
        _capture.StopRecording();
    }

    public void Dispose()
    {
        _capture.Dispose();
        _device.Dispose();
    }
}