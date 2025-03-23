```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3476)
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9290.0), X64 RyuJIT VectorSize=256


```
| Method        | Format | Mean | Error |
|-------------- |------- |-----:|------:|
| **SystemDrawing** | **Jpeg**   |   **NA** |    **NA** |
| SkiaSharp     | Jpeg   |   NA |    NA |
| LibTiff       | Jpeg   |   NA |    NA |
| MagickNet     | Jpeg   |   NA |    NA |
| **SystemDrawing** | **Tiff**   |   **NA** |    **NA** |
| SkiaSharp     | Tiff   |   NA |    NA |
| LibTiff       | Tiff   |   NA |    NA |
| MagickNet     | Tiff   |   NA |    NA |
| **SystemDrawing** | **WebP**   |   **NA** |    **NA** |
| SkiaSharp     | WebP   |   NA |    NA |
| LibTiff       | WebP   |   NA |    NA |
| MagickNet     | WebP   |   NA |    NA |

Benchmarks with issues:
  ToBinaryBenchmarks.SystemDrawing: DefaultJob [Format=Jpeg]
  ToBinaryBenchmarks.SkiaSharp: DefaultJob [Format=Jpeg]
  ToBinaryBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinaryBenchmarks.MagickNet: DefaultJob [Format=Jpeg]
  ToBinaryBenchmarks.SystemDrawing: DefaultJob [Format=Tiff]
  ToBinaryBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinaryBenchmarks.LibTiff: DefaultJob [Format=Tiff]
  ToBinaryBenchmarks.MagickNet: DefaultJob [Format=Tiff]
  ToBinaryBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinaryBenchmarks.SkiaSharp: DefaultJob [Format=WebP]
  ToBinaryBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinaryBenchmarks.MagickNet: DefaultJob [Format=WebP]
