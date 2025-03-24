```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
  [Host]               : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0             : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2
  .NET Framework 4.8.1 : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Job                  | Runtime              | Format | Mean      | Error     | StdDev     | Median    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------- |--------------------- |--------------------- |------- |----------:|----------:|-----------:|----------:|---------:|---------:|---------:|-----------:|
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Jpeg**   | **53.099 ms** | **1.6153 ms** |  **4.7629 ms** | **52.471 ms** |        **-** |        **-** |        **-** |     **1354 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Jpeg   | 62.296 ms | 2.6347 ms |  7.7684 ms | 60.897 ms |        - |        - |        - |     1540 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | Jpeg   |        NA |        NA |         NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | Jpeg   | 89.162 ms | 3.7075 ms | 10.6374 ms | 87.420 ms | 500.0000 | 500.0000 | 500.0000 | 34791339 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 60.426 ms | 3.2306 ms |  9.5255 ms | 56.716 ms |        - |        - |        - |   473316 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 68.492 ms | 3.3886 ms |  9.8310 ms | 67.431 ms |        - |        - |        - |     2731 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |        NA |        NA |         NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 90.782 ms | 2.9283 ms |  8.5882 ms | 88.923 ms | 500.0000 | 500.0000 | 500.0000 | 34792992 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Tiff**   | **23.233 ms** | **1.0063 ms** |  **2.9034 ms** | **21.904 ms** |        **-** |        **-** |        **-** |      **268 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Tiff   |        NA |        NA |         NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET 8.0             | .NET 8.0             | Tiff   |  4.121 ms | 0.1670 ms |  0.4790 ms |  3.895 ms |  15.6250 |        - |        - |   255875 B |
| MagickNet     | .NET 8.0             | .NET 8.0             | Tiff   | 59.520 ms | 3.4200 ms |  9.6463 ms | 55.631 ms | 666.6667 | 666.6667 | 666.6667 | 34790183 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   | 24.392 ms | 0.8953 ms |  2.5397 ms | 23.799 ms |        - |        - |        - |   138392 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |        NA |        NA |         NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |  4.254 ms | 0.1467 ms |  0.4326 ms |  4.060 ms |  39.0625 |   7.8125 |        - |   256424 B |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   | 61.111 ms | 2.1118 ms |  6.0929 ms | 59.742 ms | 600.0000 | 600.0000 | 600.0000 | 34793818 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **WebP**   |        **NA** |        **NA** |         **NA** |        **NA** |       **NA** |       **NA** |       **NA** |         **NA** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | WebP   | 58.757 ms | 2.0580 ms |  5.9048 ms | 56.949 ms |        - |        - |        - |     1546 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | WebP   |        NA |        NA |         NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | WebP   | 95.550 ms | 5.1050 ms | 14.5649 ms | 89.137 ms | 500.0000 | 500.0000 | 500.0000 | 34790147 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |        NA |        NA |         NA |        NA |       NA |       NA |       NA |         NA |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 65.602 ms | 3.0847 ms |  8.8009 ms | 63.200 ms |        - |        - |        - |     2458 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |        NA |        NA |         NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 99.683 ms | 3.9751 ms | 11.3411 ms | 96.210 ms | 250.0000 | 250.0000 | 250.0000 | 34794156 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
