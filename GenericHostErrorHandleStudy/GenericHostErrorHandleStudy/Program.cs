using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<ConsoleHostService>();
    });

var host = builder.Build();

var services =  host.Services.GetRequiredService<IEnumerable<IHostedService>>();

host.RunAsync(CancellationToken.None);
host.StopAsync().GetAwaiter().GetResult();


public class ConsoleHostService : IHostedService
{
    public ConsoleHostService(IStream stream)
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}