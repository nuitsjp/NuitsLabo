using System.ServiceModel.Activation;
using WfcStudy.Service;

namespace WfcStudy.ServiceHost
{
    public class EmployeeService : IEmployeeService
    {
        public Employee GetEmployee(int id)
        {
            return new Employee{Id = id, Name = "Taro Yamada"};
        }
    }
}