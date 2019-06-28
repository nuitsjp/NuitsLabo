using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Nuits.WindowsService.SimpleInjector
{
    public class TimedHostedService<TService> : IHostedService, IDisposable
        where TService : class
    {
        private readonly Container _container;
        private readonly Settings _settings;
        private readonly ILogger _logger;
        private readonly Timer _timer;

        public TimedHostedService(Container container, Settings settings, ILogger logger)
        {
            _container = container;
            _settings = settings;
            _logger = logger;
            _timer = new Timer(callback: _ => DoWork());
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 通常はSimpleInjectorのContainer#Verify()で必要なインスタンスの登録を確認する。
            // 
            // しかしここではDoWorkの中でContainerからGetInstanceしている都合上
            // Containerの初期化時にTServiceが登録されていることを保証できない。
            // 
            // ここで登録を確認しなくてもDoWorkでContainer#GetInstance()を呼び出したら
            // エラーになるが、Timerによって初回呼び出しが遅延されていた場合は、エラーが
            // 発覚するのがその分遅延してしまうため、ここでインスタンスを一度取得することで
            // 早期の登録エラーを確認する。
            _container.GetRegistration(typeof(TService), true);

            _timer.Change(dueTime: TimeSpan.Zero, period: _settings.Interval);
            return Task.CompletedTask;
        }

        private void DoWork()
        {
            try
            {
                _logger.LogInformation("DoWork");
                using (AsyncScopedLifestyle.BeginScope(_container))
                {
                    var service = _container.GetInstance<TService>();
                    _settings.Action(service);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() => _timer.Dispose();

        public class Settings
        {
            public TimeSpan DueTime { get; }
            public TimeSpan Interval { get; }
            public Action<TService> Action { get; }

            public Settings(TimeSpan dueTime, TimeSpan interval, Action<TService> action)
            {
                DueTime = dueTime;
                Interval = interval;
                Action = action;
            }
        }
    }
}