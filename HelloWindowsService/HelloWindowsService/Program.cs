using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloWindowsService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => { services.AddHostedService<LoggingService>(); });
            await builder.RunConsoleAsync();
        }
    }

    public class LoggingService : IHostedService, IDisposable
    {
        private Timer Timer { get; set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logging();
            Timer = new Timer(
                (e) => Running(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private void Running()
        {
            Logging();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logging();
            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }

        private void Logging([CallerMemberName] string memberName = null)
        {
            File.AppendAllText("log.txt", $"{DateTime.Now} - {memberName}{Environment.NewLine}");
        }
    }
}
