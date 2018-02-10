using System.ServiceModel;
// ReSharper disable InconsistentNaming

namespace AdventureWorks.Services
{
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        Employee GetEmployee(int businessEntityID);
    }
}