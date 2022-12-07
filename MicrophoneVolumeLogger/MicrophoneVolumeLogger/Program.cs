using NAudio.CoreAudioApi;

var devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToArray();

foreach (var mmDevice in devices)
{
    Console.WriteLine($"{mmDevice.FriendlyName} {mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar}");
}



void Callback(object state)
{
    Console.Write("0=" + GetBars(devices[0].AudioMeterInformation.MasterPeakValue));
    Console.Write(" ");
    Console.Write("1=" + GetBars(devices[1].AudioMeterInformation.MasterPeakValue));
    Console.Write("\n");
}

using var timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(50));

Console.ReadLine();


static string GetBars(double fraction, int barCount = 35)
{
    int barsOn = (int)(barCount * fraction);
    int barsOff = barCount - barsOn;
    return new string('#', barsOn) + new string('-', barsOff);
}
