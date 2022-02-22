using Microsoft.Extensions.Hosting;

namespace GenericHostConsoleApp;

public class ConsoleAppService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Hello, World!");
        return Task.CompletedTask;
    }
}