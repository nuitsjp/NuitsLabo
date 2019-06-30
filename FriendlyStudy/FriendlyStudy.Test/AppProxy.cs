using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Windows;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;

namespace FriendlyStudy.Test
{
    public class AppProxy : IDisposable
    {
        public AppProxy(WindowsAppFriend windowsAppFriend)
        {
            WindowsAppFriend = windowsAppFriend;
        }

        private WindowsAppFriend WindowsAppFriend { get; }

        public MainWindowProxy MainWindow => new MainWindowProxy(WindowsAppFriend.Type<Application>().Current.MainWindow);

        public static AppProxy Run()
        {
            var path = System.IO.Path.GetFullPath("FriendlyStudy.exe");
            var app = new WindowsAppFriend(Process.Start(path));
            return new AppProxy(app);
        }

        public void Dispose()
        {
            var process = Process.GetProcessById(WindowsAppFriend.ProcessId);
            WindowsAppFriend?.Dispose();
            process.CloseMainWindow();
        }
    }
}
