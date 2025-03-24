```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
  [Host]               : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0             : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2
  .NET Framework 4.8.1 : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Job                  | Runtime              | Format | Mean      | Error     | StdDev     | Median     | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------- |--------------------- |--------------------- |------- |----------:|----------:|-----------:|-----------:|---------:|---------:|---------:|-----------:|
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Jpeg**   | **59.517 ms** | **2.7299 ms** |  **8.0063 ms** |  **60.352 ms** |        **-** |        **-** |        **-** |     **1344 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Jpeg   | 64.189 ms | 2.1830 ms |  6.4366 ms |  64.457 ms |        - |        - |        - |     1546 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | Jpeg   |        NA |        NA |         NA |         NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | Jpeg   | 88.251 ms | 2.8703 ms |  8.4182 ms |  89.571 ms | 500.0000 | 500.0000 | 500.0000 | 34790147 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 62.139 ms | 3.1616 ms |  9.1724 ms |  60.799 ms |        - |        - |        - |   473088 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 68.969 ms | 3.7546 ms | 11.0706 ms |  67.477 ms |        - |        - |        - |     2048 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |        NA |        NA |         NA |         NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 95.820 ms | 3.0380 ms |  8.5688 ms |  94.787 ms | 500.0000 | 500.0000 | 500.0000 | 34792992 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Tiff**   | **24.380 ms** | **1.0583 ms** |  **3.0872 ms** |  **24.178 ms** |        **-** |        **-** |        **-** |      **268 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Tiff   |        NA |        NA |         NA |         NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET 8.0             | .NET 8.0             | Tiff   |  4.461 ms | 0.2144 ms |  0.6255 ms |   4.412 ms |  15.6250 |        - |        - |   255875 B |
| MagickNet     | .NET 8.0             | .NET 8.0             | Tiff   | 60.659 ms | 2.4405 ms |  7.0024 ms |  59.985 ms | 625.0000 | 625.0000 | 625.0000 | 34790174 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   | 25.911 ms | 1.4247 ms |  4.2007 ms |  24.578 ms |        - |        - |        - |   138392 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |        NA |        NA |         NA |         NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |  4.628 ms | 0.2263 ms |  0.6493 ms |   4.518 ms |  39.0625 |   7.8125 |        - |   256428 B |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   | 63.110 ms | 2.2589 ms |  6.6248 ms |  61.375 ms | 500.0000 | 500.0000 | 500.0000 | 34793277 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **WebP**   |        **NA** |        **NA** |         **NA** |         **NA** |       **NA** |       **NA** |       **NA** |         **NA** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | WebP   | 65.773 ms | 2.9004 ms |  8.4606 ms |  67.194 ms |        - |        - |        - |     1540 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | WebP   |        NA |        NA |         NA |         NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | WebP   | 98.540 ms | 4.4350 ms | 12.8667 ms |  97.611 ms | 571.4286 | 571.4286 | 571.4286 | 34790162 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |        NA |        NA |         NA |         NA |       NA |       NA |       NA |         NA |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 66.316 ms | 2.9373 ms |  8.6146 ms |  64.186 ms |        - |        - |        - |     1820 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |        NA |        NA |         NA |         NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 99.635 ms | 2.4864 ms |  7.0129 ms | 100.286 ms | 400.0000 | 400.0000 | 400.0000 | 34794040 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
