```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.202
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2


```
| Method        | Format | Mean        | Error      | StdDev    | Median      | Gen0        | Gen1       | Gen2       | Allocated      |
|-------------- |------- |------------:|-----------:|----------:|------------:|------------:|-----------:|-----------:|---------------:|
| **SystemDrawing** | **Jpeg**   | **1,320.37 ms** |  **92.284 ms** | **272.10 ms** | **1,438.65 ms** |           **-** |          **-** |          **-** |      **129.79 KB** |
| SkiaSharp     | Jpeg   | 1,391.64 ms |  96.331 ms | 284.04 ms | 1,532.72 ms |           - |          - |          - |      148.54 KB |
| LibTiff       | Jpeg   |          NA |         NA |        NA |          NA |          NA |         NA |         NA |             NA |
| MagickNet     | Jpeg   | 1,966.88 ms | 146.439 ms | 431.78 ms | 2,156.15 ms |  16000.0000 | 16000.0000 | 16000.0000 |  3397489.71 KB |
| Aspose        | Jpeg   | 4,170.98 ms | 254.844 ms | 751.41 ms | 4,372.30 ms | 272000.0000 | 25000.0000 | 17000.0000 | 13410589.19 KB |
| ImageSharp    | Jpeg   | 1,274.37 ms |  92.221 ms | 271.91 ms | 1,397.75 ms |   7000.0000 |  7000.0000 |  7000.0000 |   851426.32 KB |
| **SystemDrawing** | **Tiff**   |   **346.25 ms** |  **20.735 ms** |  **61.14 ms** |   **359.75 ms** |           **-** |          **-** |          **-** |       **27.45 KB** |
| SkiaSharp     | Tiff   |          NA |         NA |        NA |          NA |          NA |         NA |         NA |             NA |
| LibTiff       | Tiff   |    90.24 ms |   6.107 ms |  18.01 ms |    97.40 ms |   2000.0000 |  1714.2857 |          - |    24989.37 KB |
| MagickNet     | Tiff   | 1,526.47 ms |  95.982 ms | 283.00 ms | 1,615.04 ms |  13000.0000 | 13000.0000 | 13000.0000 |  3397471.77 KB |
| Aspose        | Tiff   | 1,145.20 ms |  83.831 ms | 247.18 ms | 1,236.11 ms |  16000.0000 | 15000.0000 | 13000.0000 |  7044035.09 KB |
| ImageSharp    | Tiff   |   957.81 ms |  65.328 ms | 192.62 ms | 1,026.62 ms |  27000.0000 | 25000.0000 | 13000.0000 |  1025062.53 KB |
| **SystemDrawing** | **WebP**   |          **NA** |         **NA** |        **NA** |          **NA** |          **NA** |         **NA** |         **NA** |             **NA** |
| SkiaSharp     | WebP   | 1,520.49 ms | 102.693 ms | 302.79 ms | 1,680.10 ms |           - |          - |          - |      148.54 KB |
| LibTiff       | WebP   |          NA |         NA |        NA |          NA |          NA |         NA |         NA |             NA |
| MagickNet     | WebP   | 2,572.62 ms | 152.643 ms | 450.07 ms | 2,808.16 ms |  16000.0000 | 16000.0000 | 16000.0000 |   3397490.9 KB |
| Aspose        | WebP   |          NA |         NA |        NA |          NA |          NA |         NA |         NA |             NA |
| ImageSharp    | WebP   | 2,391.48 ms | 150.230 ms | 442.96 ms | 2,555.17 ms |  62000.0000 | 60000.0000 | 53000.0000 |   963670.71 KB |

Benchmarks with issues:
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryMultiThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryMultiThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinaryMultiThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
