```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean         | Error        | StdDev       | Median       | Gen0    | Gen1   | Allocated |
|-------------- |------- |-------------:|-------------:|-------------:|-------------:|--------:|-------:|----------:|
| **SystemDrawing** | **Jpeg**   | **22,365.57 μs** | **1,132.322 μs** | **3,267.007 μs** | **21,242.91 μs** | **62.5000** |      **-** |  **471996 B** |
| SkiaSharp     | Jpeg   | 37,028.47 μs | 2,933.806 μs | 8,604.346 μs | 34,835.30 μs |       - |      - |         - |
| LibTiff       | Jpeg   |     65.92 μs |     3.318 μs |     9.627 μs |     63.30 μs |  1.3428 |      - |    8690 B |
| MagickNet     | Jpeg   | 47,606.39 μs | 2,668.339 μs | 7,741.336 μs | 46,017.83 μs |       - |      - |    3781 B |
| **SystemDrawing** | **Tiff**   | **21,984.51 μs** |   **690.179 μs** | **2,024.174 μs** | **21,560.83 μs** |       **-** |      **-** |  **138451 B** |
| SkiaSharp     | Tiff   |           NA |           NA |           NA |           NA |      NA |     NA |        NA |
| LibTiff       | Tiff   |     39.27 μs |     2.144 μs |     6.287 μs |     38.39 μs | 21.8506 | 0.8545 |  137830 B |
| MagickNet     | Tiff   | 14,413.48 μs |   399.417 μs | 1,152.408 μs | 14,184.33 μs |       - |      - |    3456 B |
| **SystemDrawing** | **WebP**   |           **NA** |           **NA** |           **NA** |           **NA** |      **NA** |     **NA** |        **NA** |
| SkiaSharp     | WebP   | 32,823.61 μs | 1,278.921 μs | 3,730.674 μs | 32,165.83 μs |       - |      - |         - |
| LibTiff       | WebP   |     67.04 μs |     2.199 μs |     6.239 μs |     67.93 μs |  1.3428 |      - |    8690 B |
| MagickNet     | WebP   | 49,261.60 μs | 2,164.954 μs | 6,315.273 μs | 47,816.98 μs |       - |      - |    4096 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
