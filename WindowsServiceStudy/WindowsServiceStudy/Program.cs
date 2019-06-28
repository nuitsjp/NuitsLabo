using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using WindowsServiceStudy.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Hosting;
using Nuits.WindowsService;
using Nuits.WindowsService.SimpleInjector;
using SimpleInjector;

namespace WindowsServiceStudy
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                await RunAsync(isService);
            }
            catch (Exception e)
            {
                logger.Fatal(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static async Task RunAsync(bool isService)
        {
            var container = new Container();
            var host = new HostBuilder()
                .UseNLog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSimpleInjector(container, options =>
                    {
                        // Hooks hosted services into the Generic Host pipeline
                        // while resolving them through Simple Injector
                        options.AddHostedService<TimedHostedService<IProcessor>>();
                        container.RegisterInstance(new TimedHostedService<IProcessor>.Settings(
                            dueTime: TimeSpan.Zero,
                            interval: TimeSpan.FromSeconds(1),
                            action: service => service.DoSomeWork()));
                        container.Register<IProcessor, Processor>();
                    });
                })
                .Build(isService)
                .UseSimpleInjector(container, options => 
                {
                    // Allows injection of ILogger dependencies into
                    // application components.
                    options.UseLogging();
                });
            container.Verify();
            await host.RunAsync();
        }
    }
}
