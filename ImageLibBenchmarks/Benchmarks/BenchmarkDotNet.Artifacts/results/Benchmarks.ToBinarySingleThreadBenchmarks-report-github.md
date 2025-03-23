```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]               : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0             : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET Framework 4.8.1 : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Job                  | Runtime              | Format | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------- |--------------------- |--------------------- |------- |----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Jpeg**   | **39.396 ms** | **0.7170 ms** | **0.8535 ms** |        **-** |        **-** |        **-** |     **1335 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Jpeg   | 53.950 ms | 0.4231 ms | 0.3533 ms |        - |        - |        - |     1536 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | Jpeg   | 71.032 ms | 1.2735 ms | 1.1913 ms | 571.4286 | 571.4286 | 571.4286 | 34791184 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 40.089 ms | 0.7847 ms | 1.0203 ms |        - |        - |        - |   473246 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 53.585 ms | 0.6615 ms | 0.5524 ms |        - |        - |        - |     2458 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 76.692 ms | 0.8826 ms | 0.7824 ms | 428.5714 | 428.5714 | 428.5714 | 34793234 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Tiff**   |  **3.802 ms** | **0.0248 ms** | **0.0207 ms** |        **-** |        **-** |        **-** |      **258 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET 8.0             | .NET 8.0             | Tiff   |  3.590 ms | 0.0640 ms | 0.0599 ms |  11.7188 |        - |        - |   255874 B |
| MagickNet     | .NET 8.0             | .NET 8.0             | Tiff   | 45.283 ms | 0.8992 ms | 1.9926 ms | 750.0000 | 750.0000 | 750.0000 | 34790797 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |  4.436 ms | 0.0752 ms | 0.1393 ms |  15.6250 |        - |        - |   138328 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |  4.146 ms | 0.0110 ms | 0.0092 ms |  39.0625 |   7.8125 |        - |   256424 B |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   | 47.721 ms | 0.5847 ms | 0.5183 ms | 636.3636 | 636.3636 | 636.3636 | 34795133 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **WebP**   |        **NA** |        **NA** |        **NA** |       **NA** |       **NA** |       **NA** |         **NA** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | WebP   | 51.309 ms | 0.7078 ms | 0.6621 ms |        - |        - |        - |     1536 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | WebP   | 77.205 ms | 1.5419 ms | 2.2114 ms | 571.4286 | 571.4286 | 571.4286 | 34791184 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 49.804 ms | 0.6037 ms | 0.5647 ms |        - |        - |        - |     2234 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 80.838 ms | 1.1103 ms | 0.9843 ms | 428.5714 | 428.5714 | 428.5714 | 34793234 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
