using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WindowsServiceStudy
{
    public class FileWriterService : IHostedService, IDisposable
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            WriteLog();
            _timer = new Timer(
                (e) => WriteTimeToFile(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        public void WriteTimeToFile()
        {
            WriteLog();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteLog();
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void WriteLog([CallerMemberName] string memberName = null)
        {
            Debug.WriteLine(memberName);
            Console.WriteLine(memberName);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}