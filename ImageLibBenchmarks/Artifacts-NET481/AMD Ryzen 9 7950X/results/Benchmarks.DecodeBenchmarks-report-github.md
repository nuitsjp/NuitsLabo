```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean         | Error      | StdDev     | Gen0    | Gen1   | Allocated |
|-------------- |------- |-------------:|-----------:|-----------:|--------:|-------:|----------:|
| **SystemDrawing** | **Jpeg**   | **14,233.02 μs** |  **88.561 μs** |  **82.840 μs** | **62.5000** |      **-** |  **471910 B** |
| SkiaSharp     | Jpeg   | 29,535.18 μs | 349.195 μs | 326.638 μs |       - |      - |     512 B |
| LibTiff       | Jpeg   |     39.72 μs |   0.722 μs |   0.603 μs |  1.3428 |      - |    8690 B |
| MagickNet     | Jpeg   | 37,312.28 μs | 572.902 μs | 507.862 μs |       - |      - |    3511 B |
| **SystemDrawing** | **Tiff**   |  **3,715.54 μs** |  **13.472 μs** |  **11.942 μs** | **19.5313** |      **-** |  **138232 B** |
| SkiaSharp     | Tiff   |           NA |         NA |         NA |      NA |     NA |        NA |
| LibTiff       | Tiff   |     28.21 μs |   0.559 μs |   0.523 μs | 21.8811 | 0.8850 |  137830 B |
| MagickNet     | Tiff   | 10,013.87 μs | 142.810 μs | 133.584 μs |       - |      - |    3456 B |
| **SystemDrawing** | **WebP**   |           **NA** |         **NA** |         **NA** |      **NA** |     **NA** |        **NA** |
| SkiaSharp     | WebP   | 25,752.32 μs | 305.370 μs | 285.644 μs |       - |      - |     512 B |
| LibTiff       | WebP   |     40.27 μs |   0.803 μs |   1.044 μs |  1.3428 |      - |    8690 B |
| MagickNet     | WebP   | 42,781.51 μs | 444.032 μs | 415.348 μs |       - |      - |    4096 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
