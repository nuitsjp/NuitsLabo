```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.20348.3270) (Hyper-V)
Intel Xeon Platinum 8171M CPU 2.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL


```
| Method        | Format | Mean       | Error     | StdDev    | Gen0        | Gen1       | Gen2       | Allocated      |
|-------------- |------- |-----------:|----------:|----------:|------------:|-----------:|-----------:|---------------:|
| **SystemDrawing** | **Jpeg**   | **1,696.1 ms** |  **33.76 ms** |  **42.69 ms** |           **-** |          **-** |          **-** |      **129.79 KB** |
| SkiaSharp     | Jpeg   | 2,114.9 ms |  42.01 ms |  74.68 ms |           - |          - |          - |      148.54 KB |
| LibTiff       | Jpeg   |         NA |        NA |        NA |          NA |         NA |         NA |             NA |
| MagickNet     | Jpeg   | 2,545.8 ms |  49.96 ms |  68.38 ms |  13000.0000 | 13000.0000 | 13000.0000 |  3397485.65 KB |
| Aspose        | Jpeg   | 5,393.6 ms | 103.68 ms | 106.47 ms | 192000.0000 | 32000.0000 | 21000.0000 | 13410682.07 KB |
| ImageSharp    | Jpeg   | 1,588.9 ms |  31.20 ms |  59.37 ms |   8000.0000 |  8000.0000 |  8000.0000 |   851382.79 KB |
| **SystemDrawing** | **Tiff**   |   **174.1 ms** |   **3.42 ms** |   **5.33 ms** |           **-** |          **-** |          **-** |          **27 KB** |
| SkiaSharp     | Tiff   |         NA |        NA |        NA |          NA |         NA |         NA |             NA |
| LibTiff       | Tiff   |   123.1 ms |   2.41 ms |   3.22 ms |   1200.0000 |  1000.0000 |          - |    24989.41 KB |
| MagickNet     | Tiff   | 1,819.3 ms |  32.10 ms |  26.81 ms |  11000.0000 | 11000.0000 | 11000.0000 |  3397482.25 KB |
| Aspose        | Tiff   | 1,667.1 ms |  32.82 ms |  63.23 ms |  14000.0000 | 13000.0000 | 12000.0000 |  7044001.31 KB |
| ImageSharp    | Tiff   | 1,435.0 ms |  28.49 ms |  52.09 ms |  18000.0000 | 15000.0000 |  9000.0000 |  1024584.92 KB |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |        **NA** |          **NA** |         **NA** |         **NA** |             **NA** |
| SkiaSharp     | WebP   | 1,929.9 ms |  38.42 ms |  67.29 ms |           - |          - |          - |      148.54 KB |
| LibTiff       | WebP   |         NA |        NA |        NA |          NA |         NA |         NA |             NA |
| MagickNet     | WebP   | 3,107.0 ms |  59.61 ms |  66.25 ms |  19000.0000 | 19000.0000 | 19000.0000 |   3397474.7 KB |
| Aspose        | WebP   |         NA |        NA |        NA |          NA |         NA |         NA |             NA |
| ImageSharp    | WebP   | 3,626.4 ms |  70.46 ms |  75.39 ms |  71000.0000 | 70000.0000 | 65000.0000 |   964127.05 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
