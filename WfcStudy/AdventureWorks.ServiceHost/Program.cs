using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventureWorks.Services.Impl;
using SimpleInjector.Integration.Wcf;

namespace AdventureWorks.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serviceHost =
                new SimpleInjectorServiceHost(Bootstrapper.Container, typeof(EmployeeService)))
            {
                // Open the ServiceHost to create listeners and start listening for messages.
                serviceHost.Open();

                // The service can now be accessed.
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
