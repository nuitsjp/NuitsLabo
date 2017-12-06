using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FodyApp
{
    public class MainPageViewModel
    {
        public ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();

        public string Message { get; set; }

        public MainPageViewModel()
        {
            Message = GetMessage();
        }

        public string GetMessage()
        {
            return "Hello, Fody.";
        }
    }
}
