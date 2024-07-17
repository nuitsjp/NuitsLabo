namespace WpfDataGridStudy;

public class Employee
{
    public Employee(int id, string firstName, string lastName, int age, Department department)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Age = age;
        this.Department = department;
    }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public Department Department { get; set; }
}