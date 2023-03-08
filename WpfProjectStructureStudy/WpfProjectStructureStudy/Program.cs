using System.Threading;
using WpfProjectStructureStudy.View;

if (!Thread.CurrentThread.TrySetApartmentState(ApartmentState.STA))
{
    Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
    Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
}

var app = new App();
app.Run(new MainWindow());