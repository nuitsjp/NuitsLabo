using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var f = new ChannelFactory<IEmployeeService>("EmployeeService"))
            {
                var service = f.CreateChannel();
                var employee = service.GetEmployee(100);
                Console.WriteLine($"Id:{employee.Id} Name:{employee.Name}");
                Console.ReadLine();
            }
        }
    }
}
