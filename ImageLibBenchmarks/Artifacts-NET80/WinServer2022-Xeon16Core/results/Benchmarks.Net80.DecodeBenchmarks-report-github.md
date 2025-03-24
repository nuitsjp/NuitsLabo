```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.20348.3270) (Hyper-V)
Intel Xeon Platinum 8171M CPU 2.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL


```
| Method        | Format | Mean       | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated |
|-------------- |------- |-----------:|----------:|----------:|----------:|----------:|----------:|----------:|
| **SystemDrawing** | **Jpeg**   |  **40.294 ms** | **0.7320 ms** | **0.7517 ms** |         **-** |         **-** |         **-** |     **199 B** |
| SkiaSharp     | Jpeg   |  79.546 ms | 0.4625 ms | 0.3862 ms |         - |         - |         - |     353 B |
| MagickNet     | Jpeg   |  84.606 ms | 0.3613 ms | 0.3203 ms |         - |         - |         - |    3395 B |
| Aspose        | Jpeg   |  39.662 ms | 0.1523 ms | 0.1189 ms |         - |         - |         - |     199 B |
| ImageSharp    | Jpeg   |  49.677 ms | 0.2906 ms | 0.2269 ms |         - |         - |         - |   20353 B |
| **SystemDrawing** | **Tiff**   |   **9.952 ms** | **0.0316 ms** | **0.0247 ms** |         **-** |         **-** |         **-** |     **174 B** |
| SkiaSharp     | Tiff   |         NA |        NA |        NA |        NA |        NA |        NA |        NA |
| MagickNet     | Tiff   |  28.490 ms | 0.2463 ms | 0.2183 ms |         - |         - |         - |    3340 B |
| Aspose        | Tiff   |  10.021 ms | 0.0666 ms | 0.0520 ms |         - |         - |         - |     174 B |
| ImageSharp    | Tiff   |  27.047 ms | 0.2281 ms | 0.2022 ms |   93.7500 |   31.2500 |         - | 1793517 B |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| SkiaSharp     | WebP   |  67.001 ms | 1.2982 ms | 1.9029 ms |         - |         - |         - |     346 B |
| MagickNet     | WebP   | 112.829 ms | 1.0235 ms | 0.9073 ms |         - |         - |         - |    3408 B |
| Aspose        | WebP   |         NA |        NA |        NA |        NA |        NA |        NA |        NA |
| ImageSharp    | WebP   | 169.014 ms | 2.2111 ms | 1.9600 ms | 1000.0000 | 1000.0000 | 1000.0000 | 1158813 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  DecodeBenchmarks.Aspose: DefaultJob [Format=WebP]
