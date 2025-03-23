```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  Job-FBRPBF : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256

Runtime=.NET Framework 4.8.1  InvocationCount=1  IterationCount=1  
LaunchCount=1  UnrollFactor=1  WarmupCount=1  

```
| Method        | Format | Mean        | Error | Gen0       | Gen1      | Gen2      | Allocated     |
|-------------- |------- |------------:|------:|-----------:|----------:|----------:|--------------:|
| **SystemDrawing** | **Jpeg**   |   **619.35 ms** |    **NA** | **13000.0000** |         **-** |         **-** |   **82932.61 KB** |
| SkiaSharp     | Jpeg   |   618.98 ms |    NA |          - |         - |         - |        248 KB |
| LibTiff       | Jpeg   |          NA |    NA |         NA |        NA |        NA |            NA |
| MagickNet     | Jpeg   | 1,071.98 ms |    NA |  8000.0000 | 8000.0000 | 8000.0000 | 3397796.84 KB |
| **SystemDrawing** | **Tiff**   |    **56.98 ms** |    **NA** |  **2000.0000** |         **-** |         **-** |   **14082.36 KB** |
| SkiaSharp     | Tiff   |          NA |    NA |         NA |        NA |        NA |            NA |
| LibTiff       | Tiff   |    50.51 ms |    NA |  4000.0000 | 1000.0000 |         - |    25459.2 KB |
| MagickNet     | Tiff   |   916.19 ms |    NA |  7000.0000 | 7000.0000 | 7000.0000 | 3397734.18 KB |
| **SystemDrawing** | **WebP**   |          **NA** |    **NA** |         **NA** |        **NA** |        **NA** |            **NA** |
| SkiaSharp     | WebP   |   672.41 ms |    NA |          - |         - |         - |        248 KB |
| LibTiff       | WebP   |          NA |    NA |         NA |        NA |        NA |            NA |
| MagickNet     | WebP   | 1,333.51 ms |    NA |  9000.0000 | 9000.0000 | 9000.0000 | 3397814.98 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: Job-FBRPBF(Runtime=.NET Framework 4.8.1, InvocationCount=1, IterationCount=1, LaunchCount=1, UnrollFactor=1, WarmupCount=1) [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: Job-FBRPBF(Runtime=.NET Framework 4.8.1, InvocationCount=1, IterationCount=1, LaunchCount=1, UnrollFactor=1, WarmupCount=1) [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: Job-FBRPBF(Runtime=.NET Framework 4.8.1, InvocationCount=1, IterationCount=1, LaunchCount=1, UnrollFactor=1, WarmupCount=1) [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: Job-FBRPBF(Runtime=.NET Framework 4.8.1, InvocationCount=1, IterationCount=1, LaunchCount=1, UnrollFactor=1, WarmupCount=1) [Format=WebP]
