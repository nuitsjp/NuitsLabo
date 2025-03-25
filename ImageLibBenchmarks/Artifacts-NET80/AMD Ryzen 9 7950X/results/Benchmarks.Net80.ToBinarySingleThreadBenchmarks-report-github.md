```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 9.0.200
  [Host]     : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean       | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated   |
|-------------- |------- |-----------:|----------:|----------:|----------:|----------:|----------:|------------:|
| **SystemDrawing** | **Jpeg**   |  **40.522 ms** | **0.2704 ms** | **0.2529 ms** |         **-** |         **-** |         **-** |      **1335 B** |
| SkiaSharp     | Jpeg   |  53.233 ms | 0.7426 ms | 0.6946 ms |         - |         - |         - |      1536 B |
| LibTiff       | Jpeg   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| MagickNet     | Jpeg   |  71.706 ms | 0.8119 ms | 0.7594 ms |  571.4286 |  571.4286 |  571.4286 |  34790162 B |
| Aspose        | Jpeg   | 130.671 ms | 2.1732 ms | 2.0328 ms | 2750.0000 | 1250.0000 | 1000.0000 | 137316020 B |
| ImageSharp    | Jpeg   |  35.447 ms | 0.1369 ms | 0.1213 ms |  500.0000 |  500.0000 |  500.0000 |   8722929 B |
| **SystemDrawing** | **Tiff**   |   **3.679 ms** | **0.0373 ms** | **0.0348 ms** |         **-** |         **-** |         **-** |       **258 B** |
| SkiaSharp     | Tiff   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| LibTiff       | Tiff   |   3.446 ms | 0.0576 ms | 0.0538 ms |   11.7188 |         - |         - |    255874 B |
| MagickNet     | Tiff   |  42.277 ms | 0.3214 ms | 0.3007 ms |  750.0000 |  750.0000 |  750.0000 |  34790797 B |
| Aspose        | Tiff   |  27.473 ms | 0.3436 ms | 0.3214 ms | 1000.0000 | 1000.0000 | 1000.0000 |  72120698 B |
| ImageSharp    | Tiff   |  28.569 ms | 0.5379 ms | 0.5755 ms |  625.0000 |  562.5000 |  531.2500 |  10495276 B |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |          **NA** |
| SkiaSharp     | WebP   |  48.404 ms | 0.6553 ms | 0.5809 ms |         - |         - |         - |      1532 B |
| LibTiff       | WebP   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| MagickNet     | WebP   |  74.969 ms | 1.0073 ms | 0.8929 ms |  571.4286 |  571.4286 |  571.4286 |  34790162 B |
| Aspose        | WebP   |         NA |        NA |        NA |        NA |        NA |        NA |          NA |
| ImageSharp    | WebP   |  71.089 ms | 0.3785 ms | 0.3355 ms | 1000.0000 | 1000.0000 | 1000.0000 |   9857658 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
