using System.ServiceModel.Activation;
using WfcStudy.Service;

namespace WfcStudy.Server
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class EmployeeService : IEmployeeService
    {
        public Employee GetEmployee(int id)
        {
            return new Employee{Id = id, Name = "Taro Yamada"};
        }
    }
}