using System;
using System.Linq;
using Xamarin.Forms;

namespace EventInfoAnalizerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = typeof(Page).Assembly;
            var types = assembly.ExportedTypes
                .Where(
                    x => !x.IsInterface
                        && !x.IsGenericType
                        && x.IsSubclassOf(typeof(BindableObject)))
                .OrderBy(x => x.FullName);
            foreach (var type in types)
            {
                foreach (var eventInfo in type.GetEvents().Where(x => x.DeclaringType == type).OrderBy(x => x.Name))
                {
                    Console.WriteLine($"{type.FullName} {eventInfo.Name}");
                }
            }
            Console.ReadLine();
        }
    }
}
