using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;

namespace UnitGeneratorStudy.Wpf;

public class MainWindowViewModel
{
    public DateTime DateTime { get; set; } = DateTime.Now;
    public StartDate StartDate { get; set; } = new(DateTime.Now);
    public StreetAddress StreetAddress { get; set; } = new(("Koto-ku", "Tokyo"));
    public MyDateTime? MyDateTime { get; set; }

    public Message Message { get; set; } = new("Hello, UnitGenerator");

    public ICommand MyCommand => new RelayCommand(Execute);

    private void Execute()
    {
        
    }

    public MainWindowViewModel()
    {
    }
}