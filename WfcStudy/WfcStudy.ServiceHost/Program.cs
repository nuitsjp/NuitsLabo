using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Threading;
using SimpleInjector.Integration.Wcf;

namespace WfcStudy.ServiceHost
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        static void Main()
        {
            // Create a ServiceHost for the CalculatorService type.

            using (var serviceHost = 
                new SimpleInjectorServiceHost(Bootstrapper.Container, typeof(EmployeeService)))
            {
                // Open the ServiceHost to create listeners and start listening for messages.
                serviceHost.Open();

                // The service can now be accessed.
                //Console.WriteLine("The service is ready.");
                //Console.WriteLine("Press <ENTER> to terminate service.");
                //Console.WriteLine();
                //Console.ReadLine();
                Thread.Sleep(-1);
            }

        }
    }
}
