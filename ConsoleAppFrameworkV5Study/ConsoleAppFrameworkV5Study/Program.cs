using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var app = ConsoleApp.Create()
    .ConfigureLogging(x =>
    {
        x.ClearProviders();
        x.SetMinimumLevel(LogLevel.Trace);
        x.AddSerilog();
    });

app.Add<MyCommand>();
app.Run(args);

public class MyCommand(ILogger<MyCommand> logger)
{
    [Command("")]
    public void Echo()
    {
        logger.LogInformation("Hello, World.");
    }
}