```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.20348.3270) (Hyper-V)
INTEL XEON PLATINUM 8573C, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean      | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated |
|-------------- |------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|
| **SystemDrawing** | **Jpeg**   | **22.918 ms** | **0.2027 ms** | **0.1896 ms** |         **-** |         **-** |         **-** |     **180 B** |
| SkiaSharp     | Jpeg   | 37.878 ms | 0.4517 ms | 0.4225 ms |         - |         - |         - |     325 B |
| MagickNet     | Jpeg   | 48.944 ms | 0.9366 ms | 1.1150 ms |         - |         - |         - |    3368 B |
| Aspose        | Jpeg   | 22.860 ms | 0.2085 ms | 0.1951 ms |         - |         - |         - |     180 B |
| ImageSharp    | Jpeg   | 23.064 ms | 0.4478 ms | 0.5157 ms |         - |         - |         - |   20232 B |
| **SystemDrawing** | **Tiff**   |  **5.724 ms** | **0.0466 ms** | **0.0436 ms** |         **-** |         **-** |         **-** |     **171 B** |
| SkiaSharp     | Tiff   |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| MagickNet     | Tiff   | 19.546 ms | 0.2249 ms | 0.2104 ms |         - |         - |         - |    3340 B |
| Aspose        | Tiff   |  5.720 ms | 0.0464 ms | 0.0434 ms |         - |         - |         - |     171 B |
| ImageSharp    | Tiff   | 11.379 ms | 0.0629 ms | 0.0588 ms |   15.6250 |         - |         - | 1793478 B |
| **SystemDrawing** | **WebP**   |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| SkiaSharp     | WebP   | 36.840 ms | 0.6076 ms | 0.5684 ms |         - |         - |         - |     325 B |
| MagickNet     | WebP   | 55.976 ms | 1.1153 ms | 1.9239 ms |         - |         - |         - |    3372 B |
| Aspose        | WebP   |        NA |        NA |        NA |        NA |        NA |        NA |        NA |
| ImageSharp    | WebP   | 73.879 ms | 0.4171 ms | 0.3902 ms | 1000.0000 | 1000.0000 | 1000.0000 | 1158947 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  DecodeBenchmarks.Aspose: DefaultJob [Format=WebP]
