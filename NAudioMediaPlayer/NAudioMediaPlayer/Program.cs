using NAudio.CoreAudioApi;
using NAudio.Wave;

using var emurator = new MMDeviceEnumerator();
var mmDevices = emurator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
MMDevice selectedDevice = default!;
foreach (var mmDevice in mmDevices)
{
    if (mmDevice.FriendlyName.Contains("Real"))
    {
        selectedDevice = mmDevice;
    }
}

using var player = new Player(selectedDevice);
using var waveStream = new AudioFileReader("test.wav");
// 出力に入力を接続して再生開始

var source = new CancellationTokenSource();

player.PlayLoop(waveStream, source.Token);

Console.WriteLine("\nPress Button Exit.");
Console.ReadLine();

source.Cancel();

public class Player : IDisposable
{
    private readonly MMDevice _mmDevice;
    private readonly IWavePlayer _wavePlayer;

    public Player(MMDevice mmDevice)
    {
        _mmDevice = mmDevice;
        _wavePlayer = new WasapiOut(mmDevice, AudioClientShareMode.Shared, false, 0);
    }

    public void PlayLoop(WaveStream provider, CancellationToken? token = null)
    {
        token?.Register(Stop);
        // 出力に入力を接続して再生開始
        _wavePlayer.Init(new LoopStream(provider));
        _wavePlayer.Play();
    }

    public void Stop()
    {
        _wavePlayer.Stop();
    }

    public void Dispose()
    {
        _mmDevice.Dispose();
        _wavePlayer.Dispose();
    }
}

public class LoopStream : WaveStream
{
    readonly WaveStream _sourceStream;

    public LoopStream(WaveStream sourceStream)
    {
        _sourceStream = sourceStream;
    }

    public override WaveFormat WaveFormat => _sourceStream.WaveFormat;

    public override long Length => _sourceStream.Length;

    public override long Position
    {
        get => _sourceStream.Position;
        set => _sourceStream.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var totalBytesRead = 0;

        while (totalBytesRead < count)
        {
            var bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
            if (bytesRead == 0)
            {
                if (_sourceStream.Position == 0)
                {
                    // something wrong with the source stream
                    break;
                }
                // loop
                _sourceStream.Position = 0;
            }
            totalBytesRead += bytesRead;
        }
        return totalBytesRead;
    }
}
