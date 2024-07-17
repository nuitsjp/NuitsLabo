using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var deviceEnumerator = new MMDeviceEnumerator();
        var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

        foreach (var device in devices)
        {
            Console.WriteLine($"Device: {device.FriendlyName}");
            var waveFormats = GetSupportedWaveFormats(device);

            foreach (var format in waveFormats)
            {
                Console.WriteLine($"  {format.SampleRate}Hz, {format.BitsPerSample}bit, {format.Channels} channels");
            }
            Console.WriteLine();
        }
    }

    static List<WaveFormat> GetSupportedWaveFormats(MMDevice device)
    {
        var formats = new List<WaveFormat>();

        // 一般的なサンプルレート、ビット深度、チャンネル数の組み合わせを生成
        var sampleRates = new[] { 8000, 11025, 16000, 22050, 32000, 44100, 48000, 96000, 192000 };
        var bitDepths = new[] { 8, 16, 24, 32 };
        var channelCounts = new[] { 1, 2 };

        foreach (var rate in sampleRates)
        {
            foreach (var depth in bitDepths)
            {
                foreach (var channels in channelCounts)
                {
                    try
                    {
                        var format = new WaveFormat(rate, depth, channels);
                        using (var audioClient = device.AudioClient)
                        {
                            if (audioClient.IsFormatSupported(AudioClientShareMode.Shared, format, out _))
                            {
                                formats.Add(format);
                            }
                        }
                    }
                    catch
                    {
                        // 非対応のフォーマットは無視
                    }
                }
            }
        }

        return formats;
    }
}