using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;

namespace WpfDataGridStudy;

public partial class MainWindowViewModel
{
    public ObservableCollection<Department> Departments { get; } = new();
    public ObservableCollection<Employee> Employees { get; } = new();
    public ObservableCollection<Employee> SelectedEmployees { get; private set; } = new();
    public MoveUpCommand<Employee> MoveUpCommand { get; }
    public MoveDownCommand<Employee> MoveDownCommand { get; }
    public MainWindowViewModel()
    {
        MoveUpCommand = new MoveUpCommand<Employee>(Employees, SelectedEmployees);
        MoveDownCommand = new MoveDownCommand<Employee>(Employees, SelectedEmployees);

        new []{ "HR", "IT", "Finance", "Marketing", "Engineering" }
            .Select(x => new Department(x))
            .ToList()
            .ForEach(Departments.Add);

        var firstNames = new[] { "Alice", "Bob", "Charlie", "Diana", "Eva", "Frank", "Grace", "Hank", "Ivy", "Jack" };
        var lastNames = new[] { "Anderson", "Brown", "Carter", "Davis", "Evans", "Fisher", "Green", "Harris", "Irvine", "Johnson" };

        var random = new Random();

        for (var i = 0; i < 20; i++)
        {
            var employee = new Employee(
                i + 1,
                firstNames[random.Next(firstNames.Length)],
                lastNames[random.Next(lastNames.Length)],
                random.Next(22, 60),  // Age: 22 to 59
                Departments[random.Next(Departments.Count)]
            );
            Employees.Add(employee);
        }
    }

}

public record Department(string Name);