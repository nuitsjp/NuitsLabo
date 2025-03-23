```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean      | Error     | StdDev    | Gen0    | Allocated |
|-------------- |------- |----------:|----------:|----------:|--------:|----------:|
| **SystemDrawing** | **Jpeg**   | **11.785 ms** | **0.2295 ms** | **0.3707 ms** | **62.5000** |  **471939 B** |
| SkiaSharp     | Jpeg   | 31.240 ms | 0.6199 ms | 0.9833 ms |       - |     512 B |
| **SystemDrawing** | **Tiff**   |  **3.917 ms** | **0.0392 ms** | **0.0327 ms** | **19.5313** |  **138232 B** |
| SkiaSharp     | Tiff   |        NA |        NA |        NA |      NA |        NA |
| **SystemDrawing** | **WebP**   |        **NA** |        **NA** |        **NA** |      **NA** |        **NA** |
| SkiaSharp     | WebP   | 26.543 ms | 0.5284 ms | 0.5873 ms |       - |     512 B |

Benchmarks with issues:
  DecodeBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  DecodeBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
