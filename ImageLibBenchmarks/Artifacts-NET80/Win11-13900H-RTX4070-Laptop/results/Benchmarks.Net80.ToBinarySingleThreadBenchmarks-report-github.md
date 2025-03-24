```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.202
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2


```
| Method        | Format | Mean       | Error     | StdDev     | Gen0      | Gen1      | Gen2      | Allocated   |
|-------------- |------- |-----------:|----------:|-----------:|----------:|----------:|----------:|------------:|
| **SystemDrawing** | **Jpeg**   |  **55.056 ms** | **2.1043 ms** |  **6.2046 ms** |         **-** |         **-** |         **-** |      **1354 B** |
| SkiaSharp     | Jpeg   |  59.796 ms | 1.1864 ms |  1.2694 ms |         - |         - |         - |      1540 B |
| LibTiff       | Jpeg   |         NA |        NA |         NA |        NA |        NA |        NA |          NA |
| MagickNet     | Jpeg   |  85.561 ms | 3.1730 ms |  9.2559 ms |  500.0000 |  500.0000 |  500.0000 |  34790147 B |
| Aspose        | Jpeg   | 142.754 ms | 4.1228 ms | 12.0915 ms | 3500.0000 | 1250.0000 | 1000.0000 | 137316584 B |
| ImageSharp    | Jpeg   |  47.437 ms | 1.1655 ms |  3.4365 ms |  461.5385 |  461.5385 |  461.5385 |   8720823 B |
| **SystemDrawing** | **Tiff**   |  **21.203 ms** | **0.5848 ms** |  **1.7153 ms** |         **-** |         **-** |         **-** |       **268 B** |
| SkiaSharp     | Tiff   |         NA |        NA |         NA |        NA |        NA |        NA |          NA |
| LibTiff       | Tiff   |   3.735 ms | 0.1464 ms |  0.4317 ms |   19.5313 |    3.9063 |         - |    255874 B |
| MagickNet     | Tiff   |  55.609 ms | 1.1100 ms |  1.7281 ms |  700.0000 |  700.0000 |  700.0000 |  34790190 B |
| Aspose        | Tiff   |  38.273 ms | 1.1580 ms |  3.4145 ms | 1000.0000 | 1000.0000 | 1000.0000 |  72120697 B |
| ImageSharp    | Tiff   |  34.751 ms | 1.1810 ms |  3.4075 ms |  642.8571 |  500.0000 |  500.0000 |  10494166 B |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |         **NA** |        **NA** |        **NA** |        **NA** |          **NA** |
| SkiaSharp     | WebP   |  59.879 ms | 2.1882 ms |  6.3135 ms |         - |         - |         - |      1536 B |
| LibTiff       | WebP   |         NA |        NA |         NA |        NA |        NA |        NA |          NA |
| MagickNet     | WebP   |  87.400 ms | 3.2438 ms |  9.4110 ms |  500.0000 |  500.0000 |  500.0000 |  34790147 B |
| Aspose        | WebP   |         NA |        NA |         NA |        NA |        NA |        NA |          NA |
| ImageSharp    | WebP   |  86.115 ms | 2.6689 ms |  7.8273 ms | 1000.0000 | 1000.0000 | 1000.0000 |   9857778 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
