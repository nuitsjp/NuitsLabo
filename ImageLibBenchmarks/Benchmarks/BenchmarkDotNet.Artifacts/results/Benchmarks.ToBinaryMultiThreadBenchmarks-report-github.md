```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]               : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0             : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET Framework 4.8.1 : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Job                  | Runtime              | Format | Mean        | Error     | StdDev    | Gen0       | Gen1       | Gen2       | Allocated     |
|-------------- |--------------------- |--------------------- |------- |------------:|----------:|----------:|-----------:|-----------:|-----------:|--------------:|
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Jpeg**   |   **695.85 ms** | **13.876 ms** | **27.390 ms** |          **-** |          **-** |          **-** |     **129.79 KB** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Jpeg   | 1,025.00 ms | 20.280 ms | 57.860 ms |          - |          - |          - |     148.54 KB |
| LibTiff       | .NET 8.0             | .NET 8.0             | Jpeg   |          NA |        NA |        NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | Jpeg   | 1,177.43 ms | 22.959 ms | 21.476 ms | 12000.0000 | 12000.0000 | 12000.0000 |  3397483.6 KB |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |   755.01 ms | 14.924 ms | 29.804 ms | 13000.0000 |          - |          - |   82907.98 KB |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 1,038.32 ms | 10.725 ms | 10.032 ms |          - |          - |          - |        248 KB |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |          NA |        NA |        NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 1,385.74 ms | 27.166 ms | 30.195 ms |  7000.0000 |  7000.0000 |  7000.0000 | 3397748.29 KB |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Tiff**   |    **63.09 ms** |  **1.259 ms** |  **2.892 ms** |          **-** |          **-** |          **-** |      **26.83 KB** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Tiff   |          NA |        NA |        NA |         NA |         NA |         NA |            NA |
| LibTiff       | .NET 8.0             | .NET 8.0             | Tiff   |    57.23 ms |  0.300 ms |  0.281 ms |  1538.4615 |  1153.8462 |          - |   24989.33 KB |
| MagickNet     | .NET 8.0             | .NET 8.0             | Tiff   |   826.05 ms | 16.088 ms | 20.346 ms | 13000.0000 | 13000.0000 | 13000.0000 | 3397463.29 KB |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |    68.73 ms |  0.796 ms |  0.705 ms |  2222.2222 |          - |          - |   14029.69 KB |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |          NA |        NA |        NA |         NA |         NA |         NA |            NA |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |    61.01 ms |  0.377 ms |  0.353 ms |  4100.0000 |  1800.0000 |          - |   25594.15 KB |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   | 1,037.40 ms | 20.555 ms | 34.903 ms |  9000.0000 |  9000.0000 |  9000.0000 | 3397778.18 KB |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **WebP**   |          **NA** |        **NA** |        **NA** |         **NA** |         **NA** |         **NA** |            **NA** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | WebP   |   891.91 ms | 17.785 ms | 47.472 ms |          - |          - |          - |     148.54 KB |
| LibTiff       | .NET 8.0             | .NET 8.0             | WebP   |          NA |        NA |        NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | WebP   | 1,350.09 ms | 26.905 ms | 27.629 ms | 13000.0000 | 13000.0000 | 13000.0000 | 3397481.12 KB |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |          NA |        NA |        NA |         NA |         NA |         NA |            NA |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |   904.54 ms |  9.779 ms |  9.148 ms |          - |          - |          - |        248 KB |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |          NA |        NA |        NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 1,550.71 ms | 29.745 ms | 40.715 ms | 10000.0000 | 10000.0000 | 10000.0000 | 3397761.29 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
