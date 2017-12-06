using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FodyConsole
{
    public class Sample : INotifyPropertyChanged
    {
		public string FamilyName { get; set; }

		[Interceptor]
		public string GetMessage()
		{
			return "Hello, Fody";
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
