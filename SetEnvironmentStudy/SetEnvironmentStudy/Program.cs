using BenchmarkDotNet.Running;
using SetEnvironmentStudy;

//new MyBenchmarks().SetDirectWithDemandPermission();

var summary = BenchmarkRunner.Run<MyBenchmarks>();