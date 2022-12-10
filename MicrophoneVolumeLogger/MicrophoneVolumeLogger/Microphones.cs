using NAudio.CoreAudioApi;

namespace MicrophoneVolumeLogger;

public class Microphones : IDisposable
{

    public Microphones()
    {
        Devices = new MMDeviceEnumerator()
            .EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)
            .Select(x => new Microphone(x))
            .ToArray();
    }

    public Microphone[] Devices { get; }

    public void StartRecoding()
    {
        foreach (var microphone in Devices)
        {
            microphone.StartRecoding();
        }
    }

    public void StopRecording()
    {
        foreach (var microphone in Devices)
        {
            microphone.StopRecording();
        }
    }


    public void Dispose()
    {
        foreach (var microphone in Devices)
        {
            try
            {
                microphone.Dispose();
            }
            catch
            {
                // ignore
            }
        }
    }
}