using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace WpfWithSimpleInjector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            WriteLog();
            _mainWindowViewModel = mainWindowViewModel;
            InitializeComponent();
        }

        private void WriteLog([CallerMemberName] string callerName = null)
        {
            Debug.WriteLine(callerName);
            File.AppendAllText("log.txt", $"{callerName}{Environment.NewLine}");
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnDeactivated(object? sender, EventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnFocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnGotFocus(object sender, RoutedEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnLayoutUpdated(object? sender, EventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnLostFocus(object sender, RoutedEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnSourceInitialized(object? sender, EventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnSourceUpdated(object? sender, DataTransferEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {
            WriteLog();
        }
    }
}
