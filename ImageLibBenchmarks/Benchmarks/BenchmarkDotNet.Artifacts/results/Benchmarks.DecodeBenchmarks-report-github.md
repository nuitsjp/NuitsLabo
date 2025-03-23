```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]               : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  .NET 8.0             : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET Framework 4.8.1 : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Job                  | Runtime              | Format | Mean      | Error     | StdDev    | Median    | Gen0    | Allocated |
|-------------- |--------------------- |--------------------- |------- |----------:|----------:|----------:|----------:|--------:|----------:|
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Jpeg**   | **11.165 ms** | **0.1924 ms** | **0.2291 ms** | **11.120 ms** |       **-** |     **174 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Jpeg   | 30.258 ms | 0.5389 ms | 0.5041 ms | 30.056 ms |       - |     308 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 11.570 ms | 0.2313 ms | 0.5315 ms | 11.483 ms | 62.5000 |  471939 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Jpeg   | 30.684 ms | 0.6117 ms | 1.0552 ms | 30.426 ms |       - |     512 B |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **Tiff**   |  **3.676 ms** | **0.0735 ms** | **0.1948 ms** |  **3.638 ms** |       **-** |     **170 B** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | Tiff   |        NA |        NA |        NA |        NA |      NA |        NA |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |  4.012 ms | 0.0752 ms | 0.0773 ms |  3.979 ms | 19.5313 |  138232 B |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | Tiff   |        NA |        NA |        NA |        NA |      NA |        NA |
| **SystemDrawing** | **.NET 8.0**             | **.NET 8.0**             | **WebP**   |        **NA** |        **NA** |        **NA** |        **NA** |      **NA** |        **NA** |
| SkiaSharp     | .NET 8.0             | .NET 8.0             | WebP   | 27.545 ms | 0.5449 ms | 1.3055 ms | 27.370 ms |       - |     308 B |
| SystemDrawing | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   |        NA |        NA |        NA |        NA |      NA |        NA |
| SkiaSharp     | .NET Framework 4.8.1 | .NET Framework 4.8.1 | WebP   | 26.810 ms | 0.5318 ms | 1.1783 ms | 26.387 ms |       - |     512 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: .NET 8.0(Runtime=.NET 8.0) [Format=Tiff]
  DecodeBenchmarks.SkiaSharp: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=WebP]
  DecodeBenchmarks.SystemDrawing: .NET Framework 4.8.1(Runtime=.NET Framework 4.8.1) [Format=WebP]
