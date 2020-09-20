using System;
using System.Windows;
using SimpleInjector;

namespace WpfWithSimpleInjector
{

    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();

            // Register your types, for instance:

            // Register your windows and view models:
            container.Register<MainWindow>();
            container.Register<MainWindowViewModel>();
#if DEBUG
            container.Verify();
#else
            container.Options.EnableAutoVerification = false;
#endif

            return container;
        }

        private static void RunApplication(Container container)
        {
            try
            {
                var app = new App();
                //app.InitializeComponent();
                var mainWindow = container.GetInstance<MainWindow>();
                app.Run(mainWindow);
                //app.Run(new MainWindow(new MainWindowViewModel()));
            }
            catch (Exception ex)
            {
                //Log the exception and exit
            }
        }
    }
}