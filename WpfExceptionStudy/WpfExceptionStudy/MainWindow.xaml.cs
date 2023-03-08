using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfExceptionStudy
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

        private void UiThread_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

private void Task_Click(object sender, RoutedEventArgs e)
{
    Task.Run(() =>
    {
        throw new NotImplementedException();
    });
}

        private async void AsyncVoid_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                throw new NotImplementedException();
            });
        }

        private void Thread_Click(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                throw new NotImplementedException();
            });
            thread.Start();
        }
    }
}
