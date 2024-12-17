using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using System.Text;

namespace FixedLengthFileStudy.Benchmark;

//[DryJob]
[MemoryDiagnoser]
[GcForce]
[GcConcurrent]
public class ByteStreamReaderBenchmark : IDisposable
{
    private static readonly int _lineCount = 10_000;
    private static readonly byte[] _content;

    static ByteStreamReaderBenchmark()
    {
        StringBuilder builder = new();
        for (var i = 0; i < _lineCount; i++)
        {
            builder.Append(new string('あ', 10_000));
            builder.Append("\r\n");
        }
        _content = Encoding.UTF8.GetBytes(builder.ToString());
    }

    //[Benchmark]
    //public void V001()
    //{
    //    using var reader = new V001.ByteStreamReader(new MemoryStream(_content));

    //    for (var i = 0; i < _lineCount; i++)
    //    {
    //        reader.ReadLine();
    //    }
    //}

    //[Benchmark]
    //public void V001_10K()
    //{
    //    using var reader = new V001.ByteStreamReader(new MemoryStream(_content), 10_000);

    //    for (var i = 0; i < _lineCount; i++)
    //    {
    //        reader.ReadLine();
    //    }
    //}


    //[Benchmark]
    //public void V002()
    //{
    //    using var reader = new V002.ByteStreamReader(new MemoryStream(_content));

    //    for (var i = 0; i < _lineCount; i++)
    //    {
    //        reader.ReadLine();
    //    }
    //}

    //[Benchmark]
    //public void V002_10K()
    //{
    //    using var reader = new V002.ByteStreamReader(new MemoryStream(_content), 10_000);

    //    for (var i = 0; i < _lineCount; i++)
    //    {
    //        reader.ReadLine();
    //    }
    //}

    [Benchmark]
    public void V004()
    {
        using var reader = new V004.ByteStreamReader(new MemoryStream(_content));

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V004_10K()
    {
        using var reader = new V004.ByteStreamReader(new MemoryStream(_content), 10_000);

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V005()
    {
        using var reader = new V005.ByteStreamReader(new MemoryStream(_content));

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V005_10K()
    {
        using var reader = new V005.ByteStreamReader(new MemoryStream(_content), 10_000);

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V006()
    {
        using var reader = new V006.ByteStreamReader(new MemoryStream(_content));

        for (var i = 0; i < _lineCount; i++)
        {
            reader.ReadLine();
        }
    }

    [Benchmark]
    public void V006_10K()
    {
        using var reader = new V006.ByteStreamReader(new MemoryStream(_content), 10_000);

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