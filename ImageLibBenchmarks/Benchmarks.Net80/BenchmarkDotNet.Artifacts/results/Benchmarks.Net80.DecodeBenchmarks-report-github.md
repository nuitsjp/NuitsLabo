```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.202
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2


```
| Method        | Format | Mean     | Error    | StdDev    | Median   | Gen0      | Gen1      | Gen2      | Allocated |
|-------------- |------- |---------:|---------:|----------:|---------:|----------:|----------:|----------:|----------:|
| **SystemDrawing** | **Jpeg**   | **21.48 ms** | **0.866 ms** |  **2.514 ms** | **20.64 ms** |         **-** |         **-** |         **-** |     **180 B** |
| SkiaSharp     | Jpeg   | 36.66 ms | 2.507 ms |  7.392 ms | 34.28 ms |         - |         - |         - |     327 B |
| MagickNet     | Jpeg   | 43.69 ms | 1.778 ms |  5.241 ms | 43.97 ms |         - |         - |         - |    3361 B |
| Aspose        | Jpeg   | 20.96 ms | 1.001 ms |  2.936 ms | 20.04 ms |         - |         - |         - |     180 B |
| ImageSharp    | Jpeg   | 26.38 ms | 1.608 ms |  4.717 ms | 25.29 ms |   31.2500 |   31.2500 |   31.2500 |   34484 B |
| **SystemDrawing** | **Tiff**   | **24.53 ms** | **1.293 ms** |  **3.813 ms** | **23.60 ms** |         **-** |         **-** |         **-** |     **172 B** |
| SkiaSharp     | Tiff   |       NA |       NA |        NA |       NA |        NA |        NA |        NA |        NA |
| MagickNet     | Tiff   | 15.51 ms | 0.737 ms |  2.103 ms | 14.85 ms |         - |         - |         - |    3340 B |
| Aspose        | Tiff   | 23.80 ms | 0.973 ms |  2.839 ms | 23.48 ms |         - |         - |         - |     180 B |
| ImageSharp    | Tiff   | 11.54 ms | 0.553 ms |  1.587 ms | 11.15 ms |  156.2500 |   78.1250 |   15.6250 | 1793668 B |
| **SystemDrawing** | **WebP**   |       **NA** |       **NA** |        **NA** |       **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| SkiaSharp     | WebP   | 33.37 ms | 1.602 ms |  4.647 ms | 33.39 ms |         - |         - |         - |     325 B |
| MagickNet     | WebP   | 50.94 ms | 2.725 ms |  7.991 ms | 50.04 ms |         - |         - |         - |    3378 B |
| Aspose        | WebP   |       NA |       NA |        NA |       NA |        NA |        NA |        NA |        NA |
| ImageSharp    | WebP   | 66.73 ms | 3.726 ms | 10.689 ms | 65.80 ms | 1111.1111 | 1111.1111 | 1111.1111 | 1215039 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  DecodeBenchmarks.Aspose: DefaultJob [Format=WebP]
