```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
  [Host]               : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0             : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2
  .NET Framework 4.8.1 : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Job                  | Runtime              | Format | Mean        | Error      | StdDev     | Median      | Gen0       | Gen1       | Gen2       | Allocated     |
|-------------- |--------------------- |--------------------- |------- |------------:|-----------:|-----------:|------------:|-----------:|-----------:|-----------:|--------------:|
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Jpeg**   | **1,322.83 ms** |  **96.823 ms** | **285.484 ms** | **1,178.52 ms** |          **-** |          **-** |          **-** |     **129.79 KB** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Jpeg   | 1,412.44 ms | 103.182 ms | 304.234 ms | 1,256.92 ms |          - |          - |          - |     148.54 KB |
| LibTiff       | .NET 8.0             | .NET 8.0             | Jpeg   |          NA |         NA |         NA |          NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | Jpeg   | 2,042.91 ms | 146.256 ms | 431.239 ms | 1,817.61 ms | 11000.0000 | 11000.0000 | 11000.0000 | 3397513.73 KB |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 1,457.97 ms |  98.091 ms | 289.225 ms | 1,371.47 ms | 13000.0000 |          - |          - |    81299.2 KB |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 1,497.37 ms | 110.071 ms | 324.547 ms | 1,363.32 ms |          - |          - |          - |        248 KB |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |          NA |         NA |         NA |          NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 2,397.49 ms | 173.883 ms | 512.699 ms | 2,174.92 ms | 13000.0000 | 13000.0000 | 13000.0000 |  3397918.8 KB |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Tiff**   |   **284.65 ms** |  **21.484 ms** |  **63.347 ms** |   **264.55 ms** |          **-** |          **-** |          **-** |      **27.45 KB** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Tiff   |          NA |         NA |         NA |          NA |         NA |         NA |         NA |            NA |
| LibTiff       | .NET 8.0             | .NET 8.0             | Tiff   |    83.45 ms |   5.996 ms |  17.679 ms |    69.52 ms |  2000.0000 |  1800.0000 |          - |   24989.41 KB |
| MagickNet     | .NET 8.0             | .NET 8.0             | Tiff   | 1,440.50 ms | 100.611 ms | 296.653 ms | 1,280.42 ms | 11000.0000 | 11000.0000 | 11000.0000 | 3397477.46 KB |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |   325.43 ms |  26.669 ms |  78.634 ms |   328.31 ms |  2000.0000 |          - |          - |   14053.26 KB |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |          NA |         NA |         NA |          NA |         NA |         NA |         NA |            NA |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |    74.13 ms |   1.437 ms |   1.765 ms |    73.87 ms |  4000.0000 |  2000.0000 |          - |   25422.42 KB |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   | 1,896.22 ms | 132.788 ms | 391.528 ms | 1,861.16 ms |  8000.0000 |  8000.0000 |  8000.0000 | 3397752.73 KB |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **WebP**   |          **NA** |         **NA** |         **NA** |          **NA** |         **NA** |         **NA** |         **NA** |            **NA** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | WebP   | 1,505.91 ms | 104.966 ms | 309.496 ms | 1,571.09 ms |          - |          - |          - |     148.54 KB |
| LibTiff       | .NET 8.0             | .NET 8.0             | WebP   |          NA |         NA |         NA |          NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | WebP   | 2,451.16 ms | 163.495 ms | 482.069 ms | 2,229.42 ms | 13000.0000 | 13000.0000 | 13000.0000 | 3397472.79 KB |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |          NA |         NA |         NA |          NA |         NA |         NA |         NA |            NA |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 1,511.33 ms | 110.905 ms | 327.006 ms | 1,409.92 ms |          - |          - |          - |        248 KB |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |          NA |         NA |         NA |          NA |         NA |         NA |         NA |            NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 2,863.78 ms | 184.583 ms | 544.246 ms | 2,775.78 ms |  8000.0000 |  8000.0000 |  8000.0000 | 3397797.22 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
