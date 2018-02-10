namespace AdventureWorks.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        public Employee GetEmployee(int id)
        {
            return new Employee{Id = id, Name = "Taro Yamada"};
        }
    }
}