using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfFrameStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int pageNum = 0;

        public MainWindow()
        {
            InitializeComponent();
            NavigationFrame.Navigating += OnNavigating;
            NavigationFrame.NavigationProgress += OnNavigationProgress;
            NavigationFrame.Navigated += OnNavigated;
            NavigationFrame.LoadCompleted += OnLoadCompleted;
            NavigationFrame.FragmentNavigation += OnFragmentNavigation;
            NavigationFrame.NavigationStopped += OnNavigationStopped;
            NavigationFrame.NavigationFailed += OnNavigationFailed;
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            WriteLog();
        }

        private void OnNavigationStopped(object sender, NavigationEventArgs e)
        {
            WriteLog(e.Content);
        }

        private void OnFragmentNavigation(object sender, FragmentNavigationEventArgs e)
        {
            WriteLog();
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            WriteLog(e.Content);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            WriteLog(e.Content);
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            WriteLog(e.Content);
        }

        private void OnNavigationProgress(object sender, NavigationProgressEventArgs e)
        {
            WriteLog();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new ContentPage(++pageNum));
        }

        private void WriteLog(object? page = null, [CallerMemberName] string member = "")
        {
            Debug.WriteLine($"{member} page:{(page as ContentPage)?.PageNum}");
        }

        private void OnClickNavigate(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new ContentPage(++pageNum));
        }

        private void OnClickGoBack(object sender, RoutedEventArgs e)
        {
            if (NavigationFrame.CanGoBack)
                NavigationFrame.GoBack();
        }
    }
}
