using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FodyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Main");

			var sample = new Sample();
			Console.WriteLine(sample.GetMessage());
			Console.WriteLine(sample.GetMessage());

			Console.WriteLine("End Main");
            Console.Read();
        }

		private static void Sample_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Console.WriteLine($"sender:{sender} e.PropertyName:{e.PropertyName}");
		}
	}
}
