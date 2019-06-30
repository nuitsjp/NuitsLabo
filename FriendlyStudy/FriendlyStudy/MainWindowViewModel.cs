using System.Windows.Input;
using System.Xml.Serialization;
using PropertyChanged;
using RedSheeps.Input;

namespace FriendlyStudy
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        public int Counter { get; private set; }

        public ICommand CountUpCommand => new Command(CountUp);

        private void CountUp()
        {
            Counter++;
        }
    }
}