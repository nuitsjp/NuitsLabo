```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476) (Hyper-V)
Unknown processor
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean       | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated   |
|-------------- |------- |-----------:|----------:|----------:|----------:|----------:|----------:|------------:|
| **SystemDrawing** | **Jpeg**   |  **66.297 ms** | **0.1558 ms** | **0.1457 ms** |         **-** |         **-** |         **-** |      **1348 B** |
| SkiaSharp     | Jpeg   |  71.609 ms | 0.3027 ms | 0.2683 ms |         - |         - |         - |      1553 B |
| LibTiff       | Jpeg   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| MagickNet     | Jpeg   | 100.243 ms | 0.4387 ms | 0.3663 ms |  400.0000 |  400.0000 |  400.0000 |  34790130 B |
| Aspose        | Jpeg   | 173.312 ms | 1.0869 ms | 0.8486 ms | 1333.3333 | 1000.0000 | 1000.0000 | 137316043 B |
| ImageSharp    | Jpeg   |  53.576 ms | 0.4208 ms | 0.3936 ms |  500.0000 |  500.0000 |  500.0000 |   8721268 B |
| **SystemDrawing** | **Tiff**   |  **27.273 ms** | **0.0586 ms** | **0.0548 ms** |         **-** |         **-** |         **-** |       **268 B** |
| SkiaSharp     | Tiff   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| LibTiff       | Tiff   |   4.633 ms | 0.0211 ms | 0.0197 ms |         - |         - |         - |    255875 B |
| MagickNet     | Tiff   |  69.953 ms | 0.1271 ms | 0.0992 ms |  625.0000 |  625.0000 |  625.0000 |  34790174 B |
| Aspose        | Tiff   |  47.779 ms | 0.5454 ms | 0.5102 ms | 1000.0000 | 1000.0000 | 1000.0000 |  72120748 B |
| ImageSharp    | Tiff   |  40.181 ms | 0.1755 ms | 0.1642 ms |  461.5385 |  461.5385 |  461.5385 |  10493589 B |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |          **NA** |
| SkiaSharp     | WebP   |  73.899 ms | 0.2893 ms | 0.2706 ms |         - |         - |         - |      1553 B |
| LibTiff       | WebP   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| MagickNet     | WebP   | 110.798 ms | 0.4555 ms | 0.4261 ms |  400.0000 |  400.0000 |  400.0000 |  34790130 B |
| Aspose        | WebP   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| ImageSharp    | WebP   | 110.776 ms | 0.7042 ms | 0.6587 ms | 1000.0000 | 1000.0000 | 1000.0000 |   9858731 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
