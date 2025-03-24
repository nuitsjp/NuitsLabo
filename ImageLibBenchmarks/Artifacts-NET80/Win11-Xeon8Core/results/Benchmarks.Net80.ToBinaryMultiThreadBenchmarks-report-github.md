```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476) (Hyper-V)
Unknown processor
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean        | Error     | StdDev    | Gen0       | Gen1       | Gen2       | Allocated      |
|-------------- |------- |------------:|----------:|----------:|-----------:|-----------:|-----------:|---------------:|
| **SystemDrawing** | **Jpeg**   | **1,305.74 ms** | **23.611 ms** | **20.930 ms** |          **-** |          **-** |          **-** |      **129.79 KB** |
| SkiaSharp     | Jpeg   | 1,587.01 ms | 22.481 ms | 21.029 ms |          - |          - |          - |      148.54 KB |
| LibTiff       | Jpeg   |          NA |        NA |        NA |         NA |         NA |         NA |             NA |
| MagickNet     | Jpeg   | 2,046.97 ms | 19.903 ms | 17.643 ms | 15000.0000 | 15000.0000 | 15000.0000 |  3397504.54 KB |
| Aspose        | Jpeg   | 3,710.78 ms | 27.057 ms | 23.986 ms | 55000.0000 | 22000.0000 | 18000.0000 | 13410681.38 KB |
| ImageSharp    | Jpeg   | 1,454.61 ms | 19.784 ms | 17.538 ms |  8000.0000 |  8000.0000 |  8000.0000 |    851393.7 KB |
| **SystemDrawing** | **Tiff**   |   **798.26 ms** | **15.164 ms** | **16.854 ms** |          **-** |          **-** |          **-** |       **27.45 KB** |
| SkiaSharp     | Tiff   |          NA |        NA |        NA |         NA |         NA |         NA |             NA |
| LibTiff       | Tiff   |    88.15 ms |  1.669 ms |  3.175 ms |   200.0000 |          - |          - |    24989.41 KB |
| MagickNet     | Tiff   | 1,395.03 ms | 17.202 ms | 29.211 ms | 15000.0000 | 15000.0000 | 15000.0000 |  3397550.65 KB |
| Aspose        | Tiff   |   995.12 ms |  8.281 ms | 16.538 ms | 18000.0000 | 18000.0000 | 18000.0000 |  7044111.35 KB |
| ImageSharp    | Tiff   |   921.57 ms |  7.911 ms | 15.798 ms | 11000.0000 | 10000.0000 |  9000.0000 |  1024631.93 KB |
| **SystemDrawing** | **WebP**   |          **NA** |        **NA** |        **NA** |         **NA** |         **NA** |         **NA** |             **NA** |
| SkiaSharp     | WebP   | 1,532.28 ms | 18.736 ms | 17.526 ms |          - |          - |          - |      148.54 KB |
| LibTiff       | WebP   |          NA |        NA |        NA |         NA |         NA |         NA |             NA |
| MagickNet     | WebP   | 2,254.62 ms | 26.108 ms | 24.421 ms | 15000.0000 | 15000.0000 | 15000.0000 |  3397473.82 KB |
| Aspose        | WebP   |          NA |        NA |        NA |         NA |         NA |         NA |             NA |
| ImageSharp    | WebP   | 2,194.42 ms | 23.275 ms | 21.771 ms | 21000.0000 | 20000.0000 | 20000.0000 |    962671.5 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
