using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace InfragisticsUltimateStudy.Interactivity;

public class ChromeWindowCommands
{
    public static ICommand MinimizeCommand => new RelayCommand<object>(OnMinimize);

    public static ICommand ChangeWindowStateCommand => new RelayCommand<object>(OnChangeWindowState);

    public static ICommand CloseWindowCommand => new RelayCommand<object>(OnClose);


    private static void OnChangeWindowState(object? obj)
    {
        var parentWindow = Window.GetWindow((DependencyObject)obj!);
        if (parentWindow != null)
        {
            switch (parentWindow.WindowState)
            {
                case WindowState.Maximized:
                    parentWindow.WindowState = WindowState.Normal;
                    break;
                case WindowState.Normal:
                    parentWindow.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Minimized:
                    parentWindow.WindowState = WindowState.Normal;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    private static void OnMinimize(object? obj)
    {
        var parentWindow = Window.GetWindow((DependencyObject)obj!);
        if (parentWindow != null)
            parentWindow.WindowState = WindowState.Minimized;
    }


    private static void OnClose(object? obj)
    {
        var parentWindow = Window.GetWindow((DependencyObject)obj!);
        parentWindow?.Close();
    }
}
