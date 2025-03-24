```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476) (Hyper-V)
INTEL XEON PLATINUM 8573C, 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean       | Error    | StdDev   | Gen0       | Gen1       | Gen2       | Allocated     |
|-------------- |------- |-----------:|---------:|---------:|-----------:|-----------:|-----------:|--------------:|
| **SystemDrawing** | **Jpeg**   | **1,758.5 ms** | **35.09 ms** | **41.77 ms** | **12000.0000** |          **-** |          **-** |   **74872.56 KB** |
| SkiaSharp     | Jpeg   | 2,056.8 ms | 31.75 ms | 29.70 ms |          - |          - |          - |        232 KB |
| LibTiff       | Jpeg   |         NA |       NA |       NA |         NA |         NA |         NA |            NA |
| MagickNet     | Jpeg   | 2,968.6 ms | 58.50 ms | 82.02 ms | 10000.0000 | 10000.0000 | 10000.0000 | 3397728.14 KB |
| **SystemDrawing** | **Tiff**   |   **804.0 ms** | **15.31 ms** | **14.32 ms** |  **2000.0000** |          **-** |          **-** |   **13985.84 KB** |
| SkiaSharp     | Tiff   |         NA |       NA |       NA |         NA |         NA |         NA |            NA |
| LibTiff       | Tiff   |   142.3 ms |  0.87 ms |  0.82 ms |  4000.0000 |  1750.0000 |          - |   25474.81 KB |
| MagickNet     | Tiff   | 2,181.3 ms | 43.59 ms | 61.10 ms | 15000.0000 | 15000.0000 | 15000.0000 | 3397766.53 KB |
| **SystemDrawing** | **WebP**   |         **NA** |       **NA** |       **NA** |         **NA** |         **NA** |         **NA** |            **NA** |
| SkiaSharp     | WebP   | 2,029.9 ms | 13.71 ms | 12.15 ms |          - |          - |          - |        240 KB |
| LibTiff       | WebP   |         NA |       NA |       NA |         NA |         NA |         NA |            NA |
| MagickNet     | WebP   | 3,240.7 ms | 63.07 ms | 77.46 ms | 13000.0000 | 13000.0000 | 13000.0000 | 3397866.65 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
