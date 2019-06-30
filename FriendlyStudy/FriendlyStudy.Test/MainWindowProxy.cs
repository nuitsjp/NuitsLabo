namespace FriendlyStudy.Test
{
    public class MainWindowProxy
    {
        public MainWindowProxy(dynamic mainWindow)
        {
            MainWindow = mainWindow;
        }

        private dynamic MainWindow { get; }

        public string CounterText => MainWindow.Counter.Text;

        public void CountUp()
        {
            MainWindow.CountUpButton.OnClick();
        }
    }
}