using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFodyApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		    Hoge("Hello, Interceptor");
		}

	    [Interceptor("Any valid attribute parameter types")]
        public int Hoge(string message)
	    {
	        Debug.WriteLine(message);
	        return -1;
	    }
	}
}
