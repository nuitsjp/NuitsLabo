using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using NTwain;
using NTwain.Data;
using NTwainWpfStudy.ViewModel;

namespace NTwainWpfStudy;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static readonly DependencyProperty WindowHandleProperty = DependencyProperty.Register(
        nameof(WindowHandle), typeof(IntPtr?), typeof(MainWindow), new PropertyMetadata(null));

    public IntPtr? WindowHandle
    {
        get => (IntPtr)GetValue(WindowHandleProperty);
        set => SetValue(WindowHandleProperty, value);
    }

    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
     {
        Debug.WriteLine(PlatformInfo.Current.ExpectedDsmPath);
        PlatformInfo.Current.PreferNewDSM = false;
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.Initialize(new WindowInteropHelper(this).Handle, Application.Current.Dispatcher);
        }
    }
}