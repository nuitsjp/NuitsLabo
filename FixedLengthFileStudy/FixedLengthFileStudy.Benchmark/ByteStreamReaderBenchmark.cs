using BenchmarkDotNet.Attributes;
using System.Text;
using FixedLengthFileStudy.Claude;

namespace FixedLengthFileStudy.Benchmark;

//[DryJob]
[MemoryDiagnoser]
[GcForce]
[GcConcurrent]
public class ByteStreamReaderBenchmark : IDisposable
{
    private static readonly int LineCount = 10_000;
    private static readonly byte[] Content;

    static ByteStreamReaderBenchmark()
    {
        StringBuilder builder = new();
        for (var i = 0; i < LineCount; i++)
        {
            builder.Append(new string('あ', 10_000));
            builder.Append("\r\n");
        }
        Content = Encoding.UTF8.GetBytes(builder.ToString());
    }

    private Stream _stream = Stream.Null;

    [IterationSetup]
    public void IterationSetup()
    {
        _stream = new MemoryStream(Content);
    }

    [Benchmark]
    public void V001()
    {
        using var reader = new V001.ByteStreamReader(_stream);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V001_10K()
    {
        using var reader = new V001.ByteStreamReader(_stream, 10_000);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }


    [Benchmark]
    public void V002()
    {
        using var reader = new V002.ByteStreamReader(_stream);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V002_10K()
    {
        using var reader = new V002.ByteStreamReader(_stream, 10_000);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V004()
    {
        using var reader = new V004.ByteStreamReader(_stream);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V004_10K()
    {
        using var reader = new V004.ByteStreamReader(_stream, 10_000);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V005()
    {
        using var reader = new V005.ByteStreamReader(_stream);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V005_10K()
    {
        using var reader = new V005.ByteStreamReader(_stream, 10_000);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V006()
    {
        using var reader = new V006.ByteStreamReader(_stream);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V006_10K()
    {
        using var reader = new V006.ByteStreamReader(_stream, 10_000);

        for (var i = 0; i < LineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void FixedLengthFileReader()
    {
        using var reader = new FixedLengthFileReader(_stream, Encoding.UTF8, "\r\n");

        for (var i = 0; i < LineCount; i++)
        {
            reader.Read();
            var line = reader.CurrentLine;
        }
    }

    [Benchmark]
    public void FixedLengthFileReader_10K()
    {
        using var reader = new FixedLengthFileReader(_stream, Encoding.UTF8, "\r\n", bufferSize: 10_000);

        for (var i = 0; i < LineCount; i++)
        {
            reader.Read();
            var line = reader.CurrentLine;
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}