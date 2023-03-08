using System;
using System.IO;

namespace WpfClickOnceStudy;

public class MainWindowViewModel
{
    public string Message
    {
        get
        {
            File.WriteAllText("Message.txt", $"Hello, Click Once! from File. {DateTime.Now} {MainWindow.Query}");
            return File.ReadAllText("Message.txt");
        }
    }
}