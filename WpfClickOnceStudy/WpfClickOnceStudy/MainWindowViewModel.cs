using System.IO;

namespace WpfClickOnceStudy;

public class MainWindowViewModel
{
    public string Message => File.ReadAllText("Message.txt");
}