```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean       | Error     | StdDev    | Median     | Gen0       | Gen1      | Gen2      | Allocated     |
|-------------- |------- |-----------:|----------:|----------:|-----------:|-----------:|----------:|----------:|--------------:|
| **SystemDrawing** | **Jpeg**   | **1,523.2 ms** |  **99.47 ms** | **293.30 ms** | **1,642.0 ms** | **13000.0000** |         **-** |         **-** |   **81922.53 KB** |
| SkiaSharp     | Jpeg   | 1,549.5 ms | 105.76 ms | 311.83 ms | 1,718.3 ms |          - |         - |         - |        248 KB |
| LibTiff       | Jpeg   |         NA |        NA |        NA |         NA |         NA |        NA |        NA |            NA |
| MagickNet     | Jpeg   | 2,572.2 ms | 182.99 ms | 539.56 ms | 2,802.3 ms |  8000.0000 | 8000.0000 | 8000.0000 | 3397892.26 KB |
| **SystemDrawing** | **Tiff**   |   **333.8 ms** |  **18.06 ms** |  **53.25 ms** |   **340.5 ms** |  **2000.0000** |         **-** |         **-** |   **13992.93 KB** |
| SkiaSharp     | Tiff   |         NA |        NA |        NA |         NA |         NA |        NA |        NA |            NA |
| LibTiff       | Tiff   |   100.6 ms |   6.58 ms |  19.41 ms |   108.7 ms |  4000.0000 | 2000.0000 |         - |   25398.23 KB |
| MagickNet     | Tiff   | 1,949.6 ms | 104.93 ms | 309.37 ms | 2,059.4 ms |  8000.0000 | 8000.0000 | 8000.0000 | 3397768.13 KB |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |        **NA** |         **NA** |         **NA** |        **NA** |        **NA** |            **NA** |
| SkiaSharp     | WebP   | 1,506.2 ms |  97.10 ms | 286.31 ms | 1,615.6 ms |          - |         - |         - |        248 KB |
| LibTiff       | WebP   |         NA |        NA |        NA |         NA |         NA |        NA |        NA |            NA |
| MagickNet     | WebP   | 3,023.7 ms | 192.34 ms | 567.11 ms | 3,253.1 ms |  8000.0000 | 8000.0000 | 8000.0000 | 3397756.66 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
