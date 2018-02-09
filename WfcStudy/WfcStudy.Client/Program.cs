using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using WfcStudy.Service;

namespace WfcStudy.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = new WebChannelFactory<IEmployeeService>(new Uri("http://localhost:8080/EmployeeService"));
            var service = f.CreateChannel();
            var employee = service.GetEmployee(100);
            Console.WriteLine($"Id:{employee.Id} Name:{employee.Name}");
            Console.ReadLine();
        }
    }
}
