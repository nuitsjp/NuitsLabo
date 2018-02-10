using System.ServiceModel;
using WfcStudy.Service;

namespace EmployeeService
{
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        Employee GetEmployee(int id);
    }
}