using BenchmarkDotNet.Attributes;
using System.Text;
using V001 = FixedLengthFileStudy.Benchmark.V001;
using V002 = FixedLengthFileStudy.Benchmark.V002;

namespace FixedLengthFileStudy.Benchmark;

[DryJob]
public class ByteStreamReaderBenchmark : IDisposable
{
    private static readonly int _lineCount = 10000;
    private static readonly byte[] _content;

    static ByteStreamReaderBenchmark()
    {
        StringBuilder builder = new();
        for (var i = 0; i < _lineCount; i++)
        {
            builder.Append(new string('あ', 8000));
            builder.Append("\r\n");
        }
        _content = Encoding.UTF8.GetBytes(builder.ToString());
    }

    [Benchmark]
    public void V001()
    {
        using var reader = new V001.ByteStreamReader(new MemoryStream(_content));

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V001_10K()
    {
        using var reader = new V001.ByteStreamReader(new MemoryStream(_content), 10_000);

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }


    [Benchmark]
    public void V002()
    {
        using var reader = new V002.ByteStreamReader(new MemoryStream(_content));

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V002_10K()
    {
        using var reader = new V002.ByteStreamReader(new MemoryStream(_content), 10_000);

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}