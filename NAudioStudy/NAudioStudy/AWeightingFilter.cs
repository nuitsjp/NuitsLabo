using NAudio.Dsp;
using NAudio.Wave;

namespace NAudioStudy;

class AWeightingFilter(ISampleProvider source) : ISampleProvider
{
    private readonly BiQuadFilter[] _filters = new BiQuadFilter[]
    {
        // A-weighting filter coefficients based on ITU-R 468-4 standard
        BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, 20.6f, 0.5f, -20.0f),
        BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, 107.7f, 0.5f, 2.0f),
        BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, 737.9f, 0.5f, 0.0f),
        BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, 12200.0f, 0.5f, -12.0f)
    };

    // A-weighting filter coefficients based on ITU-R 468-4 standard

    public int Read(float[] buffer, int offset, int count)
    {
        int samplesRead = source.Read(buffer, offset, count);
        for (int i = 0; i < samplesRead; i++)
        {
            float sample = buffer[offset + i];
            foreach (var filter in _filters)
            {
                sample = filter.Transform(sample);
            }
            buffer[offset + i] = sample;
        }
        return samplesRead;
    }

    public WaveFormat WaveFormat => source.WaveFormat;
}