using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWindowsService
{
    public class FileWriterService : IHostedService, IDisposable
    {
        private readonly string LogsPath;

        private readonly Timer _timer;

        public FileWriterService()
        {
            LogsPath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName)!, "Logs");
            _timer = new Timer(
                (e) => WriteTimeToFile(),
                null,
                Timeout.Infinite,
                Timeout.Infinite);

            if (Directory.Exists(LogsPath)) Directory.Delete(LogsPath, true);
            Directory.CreateDirectory(LogsPath);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Change(
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public void WriteTimeToFile()
        {
            Directory.GetFiles(LogsPath)
                .ToList()
                .ForEach(x => File.Delete(x));

            File.Create(Path.Combine(LogsPath, $"{DateTime.Now:yyyyMMdd-HHmmss}.log")).Close();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
