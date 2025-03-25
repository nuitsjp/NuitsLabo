```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean        | Error     | StdDev     | Gen0       | Gen1       | Gen2       | Allocated     |
|-------------- |------- |------------:|----------:|-----------:|-----------:|-----------:|-----------:|--------------:|
| **SystemDrawing** | **Jpeg**   |   **773.29 ms** | **15.200 ms** |  **25.396 ms** | **13000.0000** |          **-** |          **-** |   **83092.61 KB** |
| SkiaSharp     | Jpeg   |   653.86 ms | 10.904 ms |   9.666 ms |          - |          - |          - |        248 KB |
| LibTiff       | Jpeg   |          NA |        NA |         NA |         NA |         NA |         NA |            NA |
| MagickNet     | Jpeg   | 1,445.46 ms | 35.398 ms | 103.817 ms |  8000.0000 |  8000.0000 |  8000.0000 |  3397821.2 KB |
| **SystemDrawing** | **Tiff**   |    **53.91 ms** |  **0.403 ms** |   **0.315 ms** |  **2222.2222** |          **-** |          **-** |   **14042.31 KB** |
| SkiaSharp     | Tiff   |          NA |        NA |         NA |         NA |         NA |         NA |            NA |
| LibTiff       | Tiff   |    49.82 ms |  0.433 ms |   0.405 ms |  4090.9091 |  1727.2727 |          - |   25528.28 KB |
| MagickNet     | Tiff   | 1,204.85 ms | 26.048 ms |  76.804 ms |  6000.0000 |  6000.0000 |  6000.0000 | 3397734.51 KB |
| **SystemDrawing** | **WebP**   |          **NA** |        **NA** |         **NA** |         **NA** |         **NA** |         **NA** |            **NA** |
| SkiaSharp     | WebP   |   740.42 ms | 14.193 ms |  13.940 ms |          - |          - |          - |        248 KB |
| LibTiff       | WebP   |          NA |        NA |         NA |         NA |         NA |         NA |            NA |
| MagickNet     | WebP   | 1,646.72 ms | 32.695 ms |  40.152 ms | 11000.0000 | 11000.0000 | 11000.0000 | 3397863.34 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
