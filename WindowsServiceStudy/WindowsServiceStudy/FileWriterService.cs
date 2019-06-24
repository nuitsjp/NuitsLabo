using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WindowsServiceStudy
{
    public class FileWriterService : IHostedService, IDisposable
    {
        /// <summary>
        /// 定期実行処理を行うTimerオブジェクト
        /// </summary>
        private Timer _timer;

        public FileWriterService(ILogger<FileWriterService> logger)
        {
            Logger = logger;
        }

        private ILogger<FileWriterService> Logger { get; }

        /// <summary>
        /// Windowsサービスを開始する
        /// </summary>
        /// <param name="cancellationToken">開始処理自体が重い場合に、キャンセルを受け付けるためのトークン。起動処理が重くないなら使用しなくてよい。</param>
        /// <returns></returns>
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

        /// <summary>
        /// 実際のWindowsサービスの実体
        /// </summary>
        public void WriteTimeToFile()
        {
            WriteLog();
        }

        /// <summary>
        /// Windowsサービスの停止処理
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteLog();
            // Timerの次の開始時刻を無限つまり実質停止させる
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void WriteLog([CallerMemberName] string memberName = null)
        {
            Logger.LogInformation(memberName);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}