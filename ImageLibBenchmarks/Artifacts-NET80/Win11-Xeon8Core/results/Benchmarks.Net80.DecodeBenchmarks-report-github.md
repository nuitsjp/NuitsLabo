```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476) (Hyper-V)
Unknown processor
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|-------------- |------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| **SystemDrawing** | **Jpeg**   | **19.26 ms** | **0.109 ms** | **0.102 ms** |         **-** |         **-** |         **-** |     **180 B** |
| SkiaSharp     | Jpeg   | 38.03 ms | 0.124 ms | 0.116 ms |         - |         - |         - |     325 B |
| MagickNet     | Jpeg   | 48.86 ms | 0.273 ms | 0.256 ms |         - |         - |         - |    3364 B |
| Aspose        | Jpeg   | 18.97 ms | 0.101 ms | 0.089 ms |         - |         - |         - |     180 B |
| ImageSharp    | Jpeg   | 23.09 ms | 0.106 ms | 0.099 ms |         - |         - |         - |   20232 B |
| **SystemDrawing** | **Tiff**   | **26.55 ms** | **0.076 ms** | **0.071 ms** |         **-** |         **-** |         **-** |     **180 B** |
| SkiaSharp     | Tiff   |       NA |       NA |       NA |        NA |        NA |        NA |        NA |
| MagickNet     | Tiff   | 20.17 ms | 0.145 ms | 0.136 ms |         - |         - |         - |    3340 B |
| Aspose        | Tiff   | 26.56 ms | 0.068 ms | 0.064 ms |         - |         - |         - |     180 B |
| ImageSharp    | Tiff   | 10.92 ms | 0.024 ms | 0.022 ms |   15.6250 |         - |         - | 1793478 B |
| **SystemDrawing** | **WebP**   |       **NA** |       **NA** |       **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| SkiaSharp     | WebP   | 39.46 ms | 0.188 ms | 0.176 ms |         - |         - |         - |     327 B |
| MagickNet     | WebP   | 59.10 ms | 0.196 ms | 0.184 ms |         - |         - |         - |    3372 B |
| Aspose        | WebP   |       NA |       NA |       NA |        NA |        NA |        NA |        NA |
| ImageSharp    | WebP   | 77.85 ms | 0.217 ms | 0.181 ms | 1000.0000 | 1000.0000 | 1000.0000 | 1158618 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  DecodeBenchmarks.Aspose: DefaultJob [Format=WebP]
