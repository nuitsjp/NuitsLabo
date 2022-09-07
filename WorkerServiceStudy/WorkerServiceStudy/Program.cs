using System.Diagnostics;
using WorkerServiceStudy;

using var processModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
Directory.SetCurrentDirectory(Path.GetDirectoryName(processModule?.FileName)!);

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
