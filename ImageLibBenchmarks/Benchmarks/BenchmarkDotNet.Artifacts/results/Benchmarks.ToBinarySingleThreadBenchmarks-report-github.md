```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]   : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0 : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Job                | Runtime            | Format | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------- |------------------- |------------------- |------- |----------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| **SystemDrawing** | **.NET 8.0**           | **.NET 8.0**           | **Jpeg**   | **41.260 ms** | **0.8065 ms** | **1.2556 ms** |        **-** |        **-** |        **-** |     **1335 B** |
| SkiaSharp     | .NET 8.0           | .NET 8.0           | Jpeg   | 56.373 ms | 1.1221 ms | 1.2922 ms |        - |        - |        - |     1536 B |
| LibTiff       | .NET 8.0           | .NET 8.0           | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0           | .NET 8.0           | Jpeg   | 72.659 ms | 1.4028 ms | 1.1714 ms | 571.4286 | 571.4286 | 571.4286 | 34791184 B |
| SystemDrawing | .NET Framework 4.8 | .NET Framework 4.8 | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| SkiaSharp     | .NET Framework 4.8 | .NET Framework 4.8 | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET Framework 4.8 | .NET Framework 4.8 | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8 | .NET Framework 4.8 | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| **SystemDrawing** | **.NET 8.0**           | **.NET 8.0**           | **Tiff**   |  **3.897 ms** | **0.0558 ms** | **0.0522 ms** |        **-** |        **-** |        **-** |      **258 B** |
| SkiaSharp     | .NET 8.0           | .NET 8.0           | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET 8.0           | .NET 8.0           | Tiff   |  3.567 ms | 0.0284 ms | 0.0252 ms |  11.7188 |        - |        - |   255874 B |
| MagickNet     | .NET 8.0           | .NET 8.0           | Tiff   | 44.914 ms | 0.8869 ms | 0.9858 ms | 727.2727 | 727.2727 | 727.2727 | 34790847 B |
| SystemDrawing | .NET Framework 4.8 | .NET Framework 4.8 | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| SkiaSharp     | .NET Framework 4.8 | .NET Framework 4.8 | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET Framework 4.8 | .NET Framework 4.8 | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8 | .NET Framework 4.8 | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| **SystemDrawing** | **.NET 8.0**           | **.NET 8.0**           | **WebP**   |        **NA** |        **NA** |        **NA** |       **NA** |       **NA** |       **NA** |         **NA** |
| SkiaSharp     | .NET 8.0           | .NET 8.0           | WebP   | 52.706 ms | 1.0433 ms | 0.8145 ms |        - |        - |        - |     1536 B |
| LibTiff       | .NET 8.0           | .NET 8.0           | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET 8.0           | .NET 8.0           | WebP   | 78.412 ms | 1.5562 ms | 1.5981 ms | 571.4286 | 571.4286 | 571.4286 | 34791184 B |
| SystemDrawing | .NET Framework 4.8 | .NET Framework 4.8 | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| SkiaSharp     | .NET Framework 4.8 | .NET Framework 4.8 | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| LibTiff       | .NET Framework 4.8 | .NET Framework 4.8 | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |
| MagickNet     | .NET Framework 4.8 | .NET Framework 4.8 | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |         NA |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.MagickNet: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.MagickNet: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  ToBinarySingleThreadBenchmarks.SystemDrawing: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=WebP]
  ToBinarySingleThreadBenchmarks.SkiaSharp: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=WebP]
  ToBinarySingleThreadBenchmarks.MagickNet: .NET Framework 4.8(Runtime=.NET Framework 4.8) [Format=WebP]
