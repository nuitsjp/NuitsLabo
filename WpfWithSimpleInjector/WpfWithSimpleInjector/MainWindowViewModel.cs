using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace WpfWithSimpleInjector
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            WriteLog("MainWindowViewModel()");
        }
        private void WriteLog([CallerMemberName] string callerName = null)
        {
            Debug.WriteLine(callerName);
            File.AppendAllText("log.txt", $"{callerName}{Environment.NewLine}");
        }

    }
}