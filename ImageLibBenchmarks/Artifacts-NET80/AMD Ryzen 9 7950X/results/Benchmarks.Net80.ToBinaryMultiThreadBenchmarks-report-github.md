```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 9.0.200
  [Host]     : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean        | Error     | StdDev    | Gen0        | Gen1       | Gen2       | Allocated      |
|-------------- |------- |------------:|----------:|----------:|------------:|-----------:|-----------:|---------------:|
| **SystemDrawing** | **Jpeg**   |   **674.18 ms** | **13.274 ms** | **21.435 ms** |           **-** |          **-** |          **-** |      **129.79 KB** |
| SkiaSharp     | Jpeg   |   651.53 ms | 11.861 ms |  9.905 ms |           - |          - |          - |      148.54 KB |
| LibTiff       | Jpeg   |          NA |        NA |        NA |          NA |         NA |         NA |             NA |
| MagickNet     | Jpeg   |   969.46 ms | 16.386 ms | 15.327 ms |  14000.0000 | 14000.0000 | 14000.0000 |  3397488.06 KB |
| Aspose        | Jpeg   | 2,175.13 ms | 42.849 ms | 64.135 ms | 206000.0000 | 23000.0000 | 15000.0000 | 13410669.82 KB |
| ImageSharp    | Jpeg   |   515.96 ms | 10.088 ms | 15.405 ms |   9000.0000 |  9000.0000 |  9000.0000 |   851398.14 KB |
| **SystemDrawing** | **Tiff**   |    **48.40 ms** |  **0.309 ms** |  **0.289 ms** |           **-** |          **-** |          **-** |       **26.83 KB** |
| SkiaSharp     | Tiff   |          NA |        NA |        NA |          NA |         NA |         NA |             NA |
| LibTiff       | Tiff   |    41.17 ms |  0.698 ms |  0.618 ms |   1538.4615 |  1230.7692 |          - |    24989.33 KB |
| MagickNet     | Tiff   |   762.67 ms | 15.028 ms | 15.433 ms |   9000.0000 |  9000.0000 |  9000.0000 |   3397483.7 KB |
| Aspose        | Tiff   |   435.62 ms |  8.683 ms | 17.931 ms |  12000.0000 | 11000.0000 | 10000.0000 |   7044098.1 KB |
| ImageSharp    | Tiff   |   481.85 ms |  9.267 ms | 12.371 ms |  20000.0000 | 18000.0000 | 10000.0000 |  1024674.45 KB |
| **SystemDrawing** | **WebP**   |          **NA** |        **NA** |        **NA** |          **NA** |         **NA** |         **NA** |             **NA** |
| SkiaSharp     | WebP   |   758.11 ms | 14.922 ms | 13.958 ms |           - |          - |          - |      148.54 KB |
| LibTiff       | WebP   |          NA |        NA |        NA |          NA |         NA |         NA |             NA |
| MagickNet     | WebP   | 1,288.38 ms | 25.508 ms | 30.365 ms |  12000.0000 | 12000.0000 | 12000.0000 |  3397493.48 KB |
| Aspose        | WebP   |          NA |        NA |        NA |          NA |         NA |         NA |             NA |
| ImageSharp    | WebP   | 1,078.96 ms | 17.614 ms | 19.578 ms |  38000.0000 | 37000.0000 | 31000.0000 |   962949.89 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
