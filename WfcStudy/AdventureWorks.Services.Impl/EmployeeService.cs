namespace AdventureWorks.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        public Employee GetEmployee(int id)
        {
            return new Employee{BusinessEntityID = id, LoginID = "Taro Yamada"};
        }
    }
}