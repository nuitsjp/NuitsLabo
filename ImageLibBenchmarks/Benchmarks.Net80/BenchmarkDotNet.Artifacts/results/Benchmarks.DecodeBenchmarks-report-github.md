```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5039/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.202
  [Host] : .NET 8.0.14 (8.0.1425.11118), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method        | Format | Mean | Error |
|-------------- |------- |-----:|------:|
| SystemDrawing | Jpeg   |   NA |    NA |

Benchmarks with issues:
  DecodeBenchmarks.SystemDrawing: .NET 8.0(Runtime=.NET 8.0) [Format=Jpeg]
