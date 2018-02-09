using System.ServiceModel;
using System.ServiceModel.Web;

namespace WfcStudy.Service
{
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Xml)]
        Employee GetEmployee(int id);
    }
}