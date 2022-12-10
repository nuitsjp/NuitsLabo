using MicrophoneVolumeLogger;
using NAudio.CoreAudioApi;
using Sharprompt;


using var microphones = new Microphones();

foreach (var microphone in microphones.Devices)
{
    Console.WriteLine($"{microphone.FriendlyName}");
    // マイクの入力レベルを最大にする。
    microphone.MasterVolumeLevelScalar = 1f;
}

TimeSpan interval = TimeSpan.FromSeconds(5);

var answer = Prompt.Confirm($"環境音レベルを測定します。環境音以外の音源を停止してください。測定には{interval.TotalSeconds}秒かかります。よろしいですか？");
if (answer is false)
{
    Console.WriteLine("測定を中断します。");
    return;
}


microphones.StartRecoding();


void Callback(object state)
{
    Console.Write("0=" + GetBars(microphones.Devices[0].MasterPeakValue));
    Console.Write(" ");
    Console.Write("1=" + GetBars(microphones.Devices[1].MasterPeakValue));
    Console.Write("\n");
}

using var timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(50));

Thread.Sleep(interval);

microphones.StopRecording();

var noises = microphones
    .Devices
    .Select(x => new {Name = x.FriendlyName, MaxLevel = x.MasterPeakValues.Max()})
    .ToArray();


foreach (var microphone in microphones.Devices)
{
    Console.WriteLine($"{microphone.FriendlyName} {microphone.MasterPeakValues.Average()}");
}



static string GetBars(double fraction, int barCount = 35)
{
    int barsOn = (int)(barCount * fraction);
    int barsOff = barCount - barsOn;
    return new string('#', barsOn) + new string('-', barsOff);
}

