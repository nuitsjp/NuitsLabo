using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Reactive.Bindings.Extensions;

namespace WpfNumericTextBoxStudy;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        this.ObserveProperty(x => x.Text1)
            .Subscribe(x => Debug.WriteLine($"VM Text1 changed: {x}"));
        this.ObserveProperty(x => x.Text2)
            .Subscribe(x => Debug.WriteLine($"VM Text2 changed: {x}"));
    }

    [ObservableProperty] private string _text1 = "";
    [ObservableProperty] private int? _text2;
}