```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean         | Error        | StdDev       | Gen0    | Gen1   | Allocated |
|-------------- |------- |-------------:|-------------:|-------------:|--------:|-------:|----------:|
| **SystemDrawing** | **Jpeg**   | **20,814.27 μs** |   **746.804 μs** | **2,190.247 μs** | **62.5000** |      **-** |  **471959 B** |
| SkiaSharp     | Jpeg   | 33,248.72 μs | 1,375.374 μs | 3,946.206 μs |       - |      - |     512 B |
| LibTiff       | Jpeg   |     66.30 μs |     2.986 μs |     8.710 μs |  1.3428 |      - |    8690 B |
| MagickNet     | Jpeg   | 43,284.40 μs | 1,428.626 μs | 4,098.996 μs |       - |      - |    3724 B |
| **SystemDrawing** | **Tiff**   | **23,466.09 μs** |   **762.796 μs** | **2,249.121 μs** |       **-** |      **-** |  **138392 B** |
| SkiaSharp     | Tiff   |           NA |           NA |           NA |      NA |     NA |        NA |
| LibTiff       | Tiff   |     37.01 μs |     1.472 μs |     4.339 μs | 21.8506 | 0.8545 |  137830 B |
| MagickNet     | Tiff   | 14,608.24 μs |   362.496 μs | 1,051.667 μs |       - |      - |    3456 B |
| **SystemDrawing** | **WebP**   |           **NA** |           **NA** |           **NA** |      **NA** |     **NA** |        **NA** |
| SkiaSharp     | WebP   | 31,724.80 μs | 1,330.649 μs | 3,902.564 μs |       - |      - |         - |
| LibTiff       | WebP   |     63.36 μs |     2.736 μs |     8.067 μs |  1.3428 |      - |    8690 B |
| MagickNet     | WebP   | 51,156.56 μs | 1,752.108 μs | 5,083.183 μs |       - |      - |    4096 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
