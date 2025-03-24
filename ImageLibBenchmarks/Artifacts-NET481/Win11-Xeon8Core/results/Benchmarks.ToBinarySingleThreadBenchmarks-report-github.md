```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476) (Hyper-V)
INTEL XEON PLATINUM 8573C, 1 CPU, 8 logical and 4 physical cores
  [Host]               : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0             : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET Framework 4.8.1 : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Job                  | Runtime              | Format | Mean       | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------- |--------------------- |--------------------- |------- |-----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Jpeg**   |  **66.752 ms** | **0.2069 ms** | **0.1935 ms** |        **-** |        **-** |        **-** |     **1348 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Jpeg   |  70.435 ms | 0.2776 ms | 0.2596 ms |        - |        - |        - |     1553 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | Jpeg   |         NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | Jpeg   | 100.894 ms | 0.6141 ms | 0.5444 ms | 400.0000 | 400.0000 | 400.0000 | 34790130 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |  63.413 ms | 0.5521 ms | 0.5164 ms |        - |        - |        - |   473088 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |  72.904 ms | 0.2669 ms | 0.2366 ms |        - |        - |        - |     2341 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   |         NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 104.835 ms | 0.2921 ms | 0.2732 ms | 400.0000 | 400.0000 | 400.0000 | 34794040 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Tiff**   |  **27.276 ms** | **0.0828 ms** | **0.0774 ms** |        **-** |        **-** |        **-** |      **268 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Tiff   |         NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET 8.0             | .NET 8.0             | Tiff   |   5.264 ms | 0.0097 ms | 0.0086 ms |        - |        - |        - |   255875 B |
| MagickNet     | .NET 8.0             | .NET 8.0             | Tiff   |  69.640 ms | 0.3004 ms | 0.2810 ms | 571.4286 | 571.4286 | 571.4286 | 34790162 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |  27.915 ms | 0.0671 ms | 0.0595 ms |        - |        - |        - |   138392 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |         NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |   6.180 ms | 0.0164 ms | 0.0153 ms |  39.0625 |   7.8125 |        - |   256411 B |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |  74.927 ms | 0.2807 ms | 0.2488 ms | 428.5714 | 428.5714 | 428.5714 | 34793234 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **WebP**   |         **NA** |        **NA** |        **NA** |       **NA** |       **NA** |       **NA** |         **NA** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | WebP   |  74.091 ms | 0.2688 ms | 0.2383 ms |        - |        - |        - |     1553 B |
| LibTiff       | .NET 8.0             | .NET 8.0             | WebP   |         NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0             | .NET 8.0             | WebP   | 111.587 ms | 0.8446 ms | 0.7487 ms | 400.0000 | 400.0000 | 400.0000 | 34790130 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |         NA |        NA |        NA |       NA |       NA |       NA |         NA |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |  75.370 ms | 0.3110 ms | 0.2909 ms |        - |        - |        - |     2341 B |
| LibTiff       | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |         NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 115.563 ms | 0.4057 ms | 0.3795 ms | 400.0000 | 400.0000 | 400.0000 | 34794040 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
