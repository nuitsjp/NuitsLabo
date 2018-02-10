using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
