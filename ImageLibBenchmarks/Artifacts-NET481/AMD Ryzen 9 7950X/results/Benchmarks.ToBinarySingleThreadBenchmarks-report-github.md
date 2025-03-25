```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean      | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated   |
|-------------- |------- |----------:|----------:|----------:|---------:|---------:|---------:|------------:|
| **SystemDrawing** | **Jpeg**   | **40.778 ms** | **0.1897 ms** | **0.1774 ms** |        **-** |        **-** |        **-** |   **462.15 KB** |
| SkiaSharp     | Jpeg   | 52.742 ms | 1.0067 ms | 0.9888 ms |        - |        - |        - |      2.4 KB |
| LibTiff       | Jpeg   |        NA |        NA |        NA |       NA |       NA |       NA |          NA |
| MagickNet     | Jpeg   | 77.497 ms | 0.3175 ms | 0.2815 ms | 428.5714 | 428.5714 | 428.5714 | 33977.77 KB |
| **SystemDrawing** | **Tiff**   |  **4.122 ms** | **0.0192 ms** | **0.0170 ms** |  **15.6250** |        **-** |        **-** |   **135.09 KB** |
| SkiaSharp     | Tiff   |        NA |        NA |        NA |       NA |       NA |       NA |          NA |
| LibTiff       | Tiff   |  4.091 ms | 0.0145 ms | 0.0128 ms |  39.0625 |   7.8125 |        - |   250.41 KB |
| MagickNet     | Tiff   | 48.612 ms | 0.7273 ms | 0.6073 ms | 636.3636 | 636.3636 | 636.3636 |    33978 KB |
| **SystemDrawing** | **WebP**   |        **NA** |        **NA** |        **NA** |       **NA** |       **NA** |       **NA** |          **NA** |
| SkiaSharp     | WebP   | 46.619 ms | 0.3966 ms | 0.3710 ms |        - |        - |        - |     2.18 KB |
| LibTiff       | WebP   |        NA |        NA |        NA |       NA |       NA |       NA |          NA |
| MagickNet     | WebP   | 80.201 ms | 0.4627 ms | 0.3864 ms | 428.5714 | 428.5714 | 428.5714 | 33977.77 KB |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
