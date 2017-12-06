using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealProxyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateInstanceByManual();
            var usecase = new Usecase();
            var result = usecase.Add(1, 2);
            Console.WriteLine($"result:{result}");

            var message = usecase.Message;
            Console.WriteLine($"message:{message}");

            usecase.AddToOutParameter(1, 2, out int outResult);
            Console.WriteLine($"outResult:{outResult}");

            usecase.Bar(out int first, 2, out int third, 4);
            Console.WriteLine($"first:{first} third:{third}");

            Console.ReadLine();
        }

        private static void CreateInstanceByManual()
        {
            var proxy = new AspectWeavingProxy(new Usecase());
            var usecase = (Usecase) proxy.GetTransparentProxy();
            var result = usecase.Add(1, 2);
            Console.WriteLine($"result:{result}");

            var message = usecase.Message;
            Console.WriteLine($"message:{message}");

            usecase.AddToOutParameter(1, 2, out int outResult);
            Console.WriteLine($"outResult:{outResult}");

            usecase.Bar(out int first, 2, out int third, 4);
            Console.WriteLine($"first:{first} third:{third}");

            Console.ReadLine();
        }
    }
}
