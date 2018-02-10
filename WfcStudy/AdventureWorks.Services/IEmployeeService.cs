using System.ServiceModel;

namespace AdventureWorks.Services
{
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        Employee GetEmployee(int id);
    }
}