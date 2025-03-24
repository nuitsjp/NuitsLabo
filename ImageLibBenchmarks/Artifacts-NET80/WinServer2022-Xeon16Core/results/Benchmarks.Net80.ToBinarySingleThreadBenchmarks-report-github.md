```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.20348.3270) (Hyper-V)
Intel Xeon Platinum 8171M CPU 2.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL


```
| Method        | Format | Mean       | Error     | StdDev    | Median     | Gen0      | Gen1      | Gen2      | Allocated   |
|-------------- |------- |-----------:|----------:|----------:|-----------:|----------:|----------:|----------:|------------:|
| **SystemDrawing** | **Jpeg**   | **113.180 ms** | **2.2513 ms** | **2.6800 ms** | **112.251 ms** |         **-** |         **-** |         **-** |      **1384 B** |
| SkiaSharp     | Jpeg   | 140.995 ms | 2.0340 ms | 1.8031 ms | 140.471 ms |         - |         - |         - |      1596 B |
| LibTiff       | Jpeg   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| MagickNet     | Jpeg   | 184.445 ms | 3.6591 ms | 6.5980 ms | 180.764 ms |  333.3333 |  333.3333 |  333.3333 |  34790157 B |
| Aspose        | Jpeg   | 342.561 ms | 6.2236 ms | 5.8215 ms | 340.425 ms | 2000.0000 | 1000.0000 | 1000.0000 | 137316808 B |
| ImageSharp    | Jpeg   | 112.641 ms | 1.7919 ms | 1.5884 ms | 112.800 ms |  400.0000 |  400.0000 |  400.0000 |   8722163 B |
| **SystemDrawing** | **Tiff**   |  **10.982 ms** | **0.1701 ms** | **0.1508 ms** |  **10.917 ms** |         **-** |         **-** |         **-** |       **262 B** |
| SkiaSharp     | Tiff   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| LibTiff       | Tiff   |   8.638 ms | 0.1459 ms | 0.1433 ms |   8.629 ms |         - |         - |         - |    255878 B |
| MagickNet     | Tiff   | 125.826 ms | 2.4682 ms | 4.3873 ms | 124.040 ms |  250.0000 |  250.0000 |  250.0000 |  34790098 B |
| Aspose        | Tiff   | 105.757 ms | 2.1081 ms | 2.2557 ms | 105.018 ms | 1000.0000 | 1000.0000 | 1000.0000 |  72120838 B |
| ImageSharp    | Tiff   |  93.402 ms | 1.5855 ms | 1.4055 ms |  93.106 ms |  428.5714 |  428.5714 |  428.5714 |  10493097 B |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |        **NA** |         **NA** |        **NA** |        **NA** |        **NA** |          **NA** |
| SkiaSharp     | WebP   | 132.322 ms | 2.4373 ms | 2.2798 ms | 131.690 ms |         - |         - |         - |      1596 B |
| LibTiff       | WebP   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| MagickNet     | WebP   | 214.161 ms | 4.2517 ms | 6.6194 ms | 213.448 ms |  333.3333 |  333.3333 |  333.3333 |  34790157 B |
| Aspose        | WebP   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| ImageSharp    | WebP   | 236.220 ms | 4.0035 ms | 3.5490 ms | 236.829 ms | 1000.0000 | 1000.0000 | 1000.0000 |   9859299 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
