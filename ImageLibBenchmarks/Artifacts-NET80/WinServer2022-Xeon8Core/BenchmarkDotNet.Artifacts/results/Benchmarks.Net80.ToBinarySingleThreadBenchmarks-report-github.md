```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.20348.3270) (Hyper-V)
INTEL XEON PLATINUM 8573C, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.407
  [Host]     : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method        | Format | Mean       | Error     | StdDev    | Median     | Gen0      | Gen1      | Gen2      | Allocated   |
|-------------- |------- |-----------:|----------:|----------:|-----------:|----------:|----------:|----------:|------------:|
| **SystemDrawing** | **Jpeg**   |  **65.191 ms** | **0.2844 ms** | **0.2661 ms** |  **65.187 ms** |         **-** |         **-** |         **-** |      **1348 B** |
| SkiaSharp     | Jpeg   |  70.241 ms | 0.9778 ms | 0.9146 ms |  69.947 ms |         - |         - |         - |      1546 B |
| LibTiff       | Jpeg   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| MagickNet     | Jpeg   | 101.435 ms | 2.0186 ms | 4.2136 ms |  99.707 ms |  500.0000 |  500.0000 |  500.0000 |  34790147 B |
| Aspose        | Jpeg   | 167.711 ms | 3.1334 ms | 3.2178 ms | 167.163 ms | 1333.3333 | 1000.0000 | 1000.0000 | 137316035 B |
| ImageSharp    | Jpeg   |  53.087 ms | 0.6170 ms | 0.5470 ms |  52.983 ms |  444.4444 |  444.4444 |  444.4444 |   8721641 B |
| **SystemDrawing** | **Tiff**   |   **6.364 ms** | **0.0467 ms** | **0.0390 ms** |   **6.374 ms** |         **-** |         **-** |         **-** |       **259 B** |
| SkiaSharp     | Tiff   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| LibTiff       | Tiff   |   4.871 ms | 0.0310 ms | 0.0275 ms |   4.858 ms |         - |         - |         - |    255875 B |
| MagickNet     | Tiff   |  67.949 ms | 1.3135 ms | 1.4600 ms |  68.074 ms |  625.0000 |  625.0000 |  625.0000 |  34790174 B |
| Aspose        | Tiff   |  45.641 ms | 0.4886 ms | 0.4332 ms |  45.624 ms | 1000.0000 | 1000.0000 | 1000.0000 |  72120703 B |
| ImageSharp    | Tiff   |  40.094 ms | 0.4474 ms | 0.3736 ms |  40.003 ms |  461.5385 |  461.5385 |  461.5385 |  10493862 B |
| **SystemDrawing** | **WebP**   |         **NA** |        **NA** |        **NA** |         **NA** |        **NA** |        **NA** |        **NA** |          **NA** |
| SkiaSharp     | WebP   |  67.962 ms | 1.2974 ms | 1.2136 ms |  67.771 ms |         - |         - |         - |      1546 B |
| LibTiff       | WebP   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| MagickNet     | WebP   | 107.949 ms | 2.1028 ms | 2.2500 ms | 108.476 ms |  400.0000 |  400.0000 |  400.0000 |  34790130 B |
| Aspose        | WebP   |         NA |        NA |        NA |         NA |        NA |        NA |        NA |          NA |
| ImageSharp    | WebP   | 105.665 ms | 0.8209 ms | 0.7277 ms | 105.676 ms | 1000.0000 | 1000.0000 | 1000.0000 |   9857987 B |

Benchmarks with issues:
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=Jpeg]
  ToBinarySingleThreadBenchmarks.SkiaSharp: DefaultJob [Format=Tiff]
  ToBinarySingleThreadBenchmarks.SystemDrawing: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.LibTiff: DefaultJob [Format=WebP]
  ToBinarySingleThreadBenchmarks.Aspose: DefaultJob [Format=WebP]
