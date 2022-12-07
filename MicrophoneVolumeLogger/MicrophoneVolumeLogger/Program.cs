//using NAudio.Wave;

//int waveInDevices = WaveIn.DeviceCount;
//for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
//{
//    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
//    Console.WriteLine("Device {0}: {1}, {2} channels", waveInDevice, deviceInfo.ProductName, deviceInfo.Channels);
//}

//var waveIn = new NAudio.Wave.WaveInEvent
//{
//    DeviceNumber = 0, // customize this to select your microphone device
//    WaveFormat = new NAudio.Wave.WaveFormat(rate: 44100, bits: 16, channels: 1),
//    BufferMilliseconds = 50
//};
//waveIn.DataAvailable += ShowPeakMono;
//waveIn.StartRecording();

//Console.ReadLine();

//static void ShowPeakMono(object sender, NAudio.Wave.WaveInEventArgs args)
//{
//    float maxValue = 32767;
//    int peakValue = 0;
//    int bytesPerSample = 2;
//    for (int index = 0; index < args.BytesRecorded; index += bytesPerSample)
//    {
//        int value = BitConverter.ToInt16(args.Buffer, index);
//        peakValue = Math.Max(peakValue, value);
//    }

//    Console.WriteLine("L=" + GetBars(peakValue / maxValue));
//}

//static string GetBars(double fraction, int barCount = 35)
//{
//    int barsOn = (int)(barCount * fraction);
//    int barsOff = barCount - barsOn;
//    return new string('#', barsOn) + new string('-', barsOff);
//}

// Get the default audio input device

using NAudio.CoreAudioApi;

var devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToArray();

foreach (var mmDevice in devices)
{
    Console.WriteLine($"{mmDevice.FriendlyName} {mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar}");
    //mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar = 1.0f;

}



void Callback(object state)
{
    Console.Write("0=" + GetBars(devices[0].AudioMeterInformation.MasterPeakValue));
    Console.Write(" ");
    Console.Write("1=" + GetBars(devices[1].AudioMeterInformation.MasterPeakValue));
    Console.Write("\n");
}

var timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(50));

Console.ReadLine();


static string GetBars(double fraction, int barCount = 35)
{
    int barsOn = (int)(barCount * fraction);
    int barsOff = barCount - barsOn;
    return new string('#', barsOn) + new string('-', barsOff);
}