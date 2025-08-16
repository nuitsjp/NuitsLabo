using ConsoleAppFramework;
using ConsoleAppFrameworkStudy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

// Serilogの設定 - コンソール出力のフォーマットを指定
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

try
{
    var builder = Host.CreateApplicationBuilder();

    // SerilogをDIコンテナに登録
    builder.Services.AddSerilog();
    builder.Services.AddTransient<IFooService, FooService>();

    var app = builder.ToConsoleAppBuilder();
    app.Add<FooApp>();
    await app.RunAsync(args);
}
catch (Exception ex)
{
    Log.Fatal(ex, "アプリケーションの実行中に致命的なエラーが発生しました");
}
finally
{
    Log.CloseAndFlush();
    Console.WriteLine($"ExitCode:{Environment.ExitCode}");
}

//try
//{
//    var app = ConsoleApp.Create()
//        .ConfigureDefaultConfiguration()
//        .ConfigureServices(service =>
//        {
//            service.AddTransient<IFooService, FooService>();
//        });
//    app.Add<FooApp>();
//    await app.RunAsync(args);
//}
//finally
//{
//    Console.WriteLine($"ExitCode:{Environment.ExitCode}");
//}
