using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using Microsoft.Win32;

namespace SetEnvironmentStudy;

[SimpleJob(RunStrategy.ColdStart,
    launchCount: 1,     // プロセス起動は1回
    warmupCount: 1,     // ウォームアップも1回
    iterationCount: 3,  // 3回程度の計測
    invocationCount: 1  // 各イテレーションでの実行回数
)]
public class MyBenchmarks : IDisposable
{
    private const string Key = nameof(SetEnvironmentStudy);

    public MyBenchmarks()
    {
        //Environment.SetEnvironmentVariable(Key, null, EnvironmentVariableTarget.Process);
        //Environment.SetEnvironmentVariable(Key, null, EnvironmentVariableTarget.User);
        // Environment.SetEnvironmentVariable(Key, null, EnvironmentVariableTarget.Machine);
    }

    [Benchmark]
    public void SetProcess()
    {
        Environment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.Process);
    }


    [Benchmark]
    public void SetUser()
    {
        Environment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.User);
    }

    [Benchmark]
    public void SetDirectWithDemandPermission()
    {
        MyEnvironment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.User, true, true);
    }

    [Benchmark]
    public void SetDirectWithNotify()
    {
        MyEnvironment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.User, false, true);
    }

    [Benchmark]
    public void SetDirectWithoutNotify()
    {
        MyEnvironment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.User, false, false);
    }

    [Benchmark]
    public void SetDirectSkipHung()
    {
        MyEnvironment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.User, true, true, true);
    }

    [Benchmark]
    public void SetDirectNotSkipHung()
    {
        MyEnvironment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.User, true, true, true, true);
    }

    //[Benchmark]
    //public void SetRegistry()
    //{
    //    SetUserEnvironmentVariable(Key, "Hello, World!");
    //}

    //[Benchmark]
    //public void SetMachine()
    //{
    //    Environment.SetEnvironmentVariable(Key, "Hello, World!", EnvironmentVariableTarget.Machine);
    //}

    public void Dispose()
    {
        Environment.SetEnvironmentVariable(Key, null, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable(Key, null, EnvironmentVariableTarget.User);
        // Environment.SetEnvironmentVariable(Key, null, EnvironmentVariableTarget.Machine);
    }

    private static void SetUserEnvironmentVariable(string variable, string? value)
    {
        using RegistryKey environmentKey = Registry.CurrentUser.OpenSubKey("Environment", true)!;
        if (value == null)
            environmentKey.DeleteValue(variable, false);
        else
            environmentKey.SetValue(variable, value);
    }
}