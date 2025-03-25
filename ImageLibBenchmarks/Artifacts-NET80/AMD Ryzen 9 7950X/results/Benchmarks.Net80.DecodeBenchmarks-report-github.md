```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 9.0.200
  [Host]     : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean      | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated |
|-------------- |------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|
| **SystemDrawing** | **Jpeg**   | **13.365 ms** | **0.0645 ms** | **0.0603 ms** |         **-** |         **-** |         **-** |     **174 B** |
| SkiaSharp     | Jpeg   | 27.774 ms | 0.2107 ms | 0.1971 ms |         - |         - |         - |     308 B |
| MagickNet     | Jpeg   | 36.767 ms | 0.6850 ms | 0.6408 ms |         - |         - |         - |    3357 B |
| Aspose        | Jpeg   | 13.317 ms | 0.0738 ms | 0.0690 ms |         - |         - |         - |     174 B |
| ImageSharp    | Jpeg   | 15.133 ms | 0.1061 ms | 0.0941 ms |         - |         - |         - |   20200 B |
| **SystemDrawing** | **Tiff**   |  **3.357 ms** | **0.0240 ms** | **0.0213 ms** |         **-** |         **-** |         **-** |     **170 B** |
| SkiaSharp     | Tiff   |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| MagickNet     | Tiff   |  7.891 ms | 0.0776 ms | 0.0726 ms |         - |         - |         - |    3334 B |
| Aspose        | Tiff   |  3.307 ms | 0.0237 ms | 0.0210 ms |         - |         - |         - |     170 B |
| ImageSharp    | Tiff   |  7.367 ms | 0.1111 ms | 0.1039 ms |  101.5625 |   46.8750 |         - | 1793491 B |
| **SystemDrawing** | **WebP**   |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| SkiaSharp     | WebP   | 22.043 ms | 0.2184 ms | 0.1936 ms |         - |         - |         - |     308 B |
| MagickNet     | WebP   | 38.693 ms | 0.4927 ms | 0.4609 ms |         - |         - |         - |    3359 B |
| Aspose        | WebP   |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| ImageSharp    | WebP   | 50.019 ms | 0.3769 ms | 0.3525 ms | 1000.0000 | 1000.0000 | 1000.0000 | 1159482 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  DecodeBenchmarks.Aspose: DefaultJob [Format=WebP]
