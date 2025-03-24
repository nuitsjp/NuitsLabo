```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476) (Hyper-V)
INTEL XEON PLATINUM 8573C, 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean         | Error      | StdDev     | Gen0    | Gen1   | Allocated |
|-------------- |------- |-------------:|-----------:|-----------:|--------:|-------:|----------:|
| **SystemDrawing** | **Jpeg**   | **19,596.36 μs** |  **57.670 μs** |  **51.123 μs** | **62.5000** |      **-** |  **471949 B** |
| SkiaSharp     | Jpeg   | 38,145.77 μs | 101.976 μs |  95.388 μs |       - |      - |         - |
| LibTiff       | Jpeg   |    114.69 μs |   2.288 μs |   3.946 μs |  1.3428 |      - |    8690 B |
| MagickNet     | Jpeg   | 49,058.07 μs | 166.066 μs | 155.339 μs |       - |      - |    3724 B |
| **SystemDrawing** | **Tiff**   | **27,082.28 μs** |  **58.040 μs** |  **54.291 μs** |       **-** |      **-** |  **138392 B** |
| SkiaSharp     | Tiff   |           NA |         NA |         NA |      NA |     NA |        NA |
| LibTiff       | Tiff   |     49.30 μs |   0.101 μs |   0.084 μs | 21.8506 | 0.8545 |  137830 B |
| MagickNet     | Tiff   | 19,887.87 μs | 121.345 μs | 113.506 μs |       - |      - |    3584 B |
| **SystemDrawing** | **WebP**   |           **NA** |         **NA** |         **NA** |      **NA** |     **NA** |        **NA** |
| SkiaSharp     | WebP   | 40,157.94 μs |  72.705 μs |  68.008 μs |       - |      - |         - |
| LibTiff       | WebP   |    114.32 μs |   2.267 μs |   4.831 μs |  1.3428 |      - |    8690 B |
| MagickNet     | WebP   | 59,084.11 μs | 242.240 μs | 226.591 μs |       - |      - |    3641 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
