```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.20348.3270) (Hyper-V)
INTEL XEON PLATINUM 8573C, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean       | Error    | StdDev   | Gen0       | Gen1       | Gen2       | Allocated      |
|-------------- |------- |-----------:|---------:|---------:|-----------:|-----------:|-----------:|---------------:|
| **SystemDrawing** | **Jpeg**   | **1,446.9 ms** | **28.59 ms** | **42.79 ms** |          **-** |          **-** |          **-** |      **129.79 KB** |
| SkiaSharp     | Jpeg   | 1,635.7 ms | 32.43 ms | 37.35 ms |          - |          - |          - |      148.54 KB |
| LibTiff       | Jpeg   |         NA |       NA |       NA |         NA |         NA |         NA |             NA |
| MagickNet     | Jpeg   | 2,142.1 ms | 41.24 ms | 53.62 ms | 16000.0000 | 16000.0000 | 16000.0000 |  3397526.24 KB |
| Aspose        | Jpeg   | 3,868.0 ms | 70.77 ms | 62.73 ms | 57000.0000 | 27000.0000 | 20000.0000 | 13410745.75 KB |
| ImageSharp    | Jpeg   | 1,171.6 ms | 22.28 ms | 48.91 ms | 10000.0000 | 10000.0000 | 10000.0000 |   851530.56 KB |
| **SystemDrawing** | **Tiff**   |   **169.7 ms** |  **3.37 ms** |  **3.31 ms** |          **-** |          **-** |          **-** |          **27 KB** |
| SkiaSharp     | Tiff   |         NA |       NA |       NA |         NA |         NA |         NA |             NA |
| LibTiff       | Tiff   |   113.0 ms |  1.82 ms |  1.52 ms |          - |          - |          - |    24990.58 KB |
| MagickNet     | Tiff   | 1,523.1 ms | 29.74 ms | 41.68 ms | 13000.0000 | 13000.0000 | 13000.0000 |  3397485.91 KB |
| Aspose        | Tiff   | 1,217.8 ms | 22.32 ms | 20.88 ms | 28000.0000 | 28000.0000 | 28000.0000 |  7043840.38 KB |
| ImageSharp    | Tiff   | 1,150.0 ms | 22.94 ms | 25.50 ms | 16000.0000 | 15000.0000 | 14000.0000 |  1024732.78 KB |
| **SystemDrawing** | **WebP**   |         **NA** |       **NA** |       **NA** |         **NA** |         **NA** |         **NA** |             **NA** |
| SkiaSharp     | WebP   | 1,508.3 ms | 27.00 ms | 38.72 ms |          - |          - |          - |      148.54 KB |
| LibTiff       | WebP   |         NA |       NA |       NA |         NA |         NA |         NA |             NA |
| MagickNet     | WebP   | 2,360.8 ms | 46.83 ms | 48.10 ms | 17000.0000 | 17000.0000 | 17000.0000 |  3397521.68 KB |
| Aspose        | WebP   |         NA |       NA |       NA |         NA |         NA |         NA |             NA |
| ImageSharp    | WebP   | 2,169.5 ms | 41.92 ms | 39.21 ms | 18000.0000 | 17000.0000 | 17000.0000 |    962659.2 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
