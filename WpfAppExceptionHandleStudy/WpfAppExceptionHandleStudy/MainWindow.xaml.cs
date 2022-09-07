using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppExceptionHandleStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new TestException();
        }

        private async void ThrowExceptionAsync(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => throw new TestException());
        }

        private void ThrowExceptionAsyncWithoutAwait(object sender, RoutedEventArgs e)
        {
            Task.Run(() => throw new TestException());
        }

        private void ThrowWithThreadStart(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() => throw new TestException());
            thread.Start();
        }
    }

    public class TestException : Exception
    {

    }
}
