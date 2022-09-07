using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppExceptionHandleStudy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, args) =>
            {
                Debug.WriteLine($"AppDomain.CurrentDomain.UnhandledException o:{o} args:{args.ExceptionObject}");
            };
            Dispatcher.UnhandledException += (o, args) =>
            {
                Debug.WriteLine($"Dispatcher.UnhandledException o:{o} args:{args.Exception}");
                args.Handled = true;
            };
            DispatcherUnhandledException += (o, args) =>
            {
                Debug.WriteLine($"Application.Current.DispatcherUnhandledException o:{o} args:{args.Exception}");
            };
            TaskScheduler.UnobservedTaskException += (o, args) =>
            {
                Debug.WriteLine($"TaskScheduler.UnobservedTaskException o:{o} args:{args.Exception}");
            };
        }
    }
}
