using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace MaterialDesignThemesStudy;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        new[] { "HR", "IT", "Finance", "Marketing", "Engineering" }
            .Select(x => new Department(x))
            .ToList()
            .ForEach(Departments.Add);

        Items1 = CreateData();
        Items2 = CreateData();
        Items3 = CreateData();
        Items4 = CreateData();

        foreach (var model in Items1)
        {
            model.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectableViewModel.IsSelected))
                    OnPropertyChanged(nameof(IsAllItems1Selected));
            };
        }

        Files = new List<string>();

        for (int i = 0; i < 1000; i++)
        {
            Files.Add(Path.GetRandomFileName());
        }
    }

    public bool? IsAllItems1Selected
    {
        get
        {
            var selected = Items1.Select(item => item.IsSelected).Distinct().ToList();
            return selected.Count == 1 ? selected.Single() : (bool?)null;
        }
        set
        {
            if (value.HasValue)
            {
                SelectAll(value.Value, Items1);
                OnPropertyChanged();
            }
        }
    }

    private static void SelectAll(bool select, IEnumerable<SelectableViewModel> models)
    {
        foreach (var model in models)
        {
            model.IsSelected = select;
        }
    }

    private ObservableCollection<SelectableViewModel> CreateData()
    {

        return new ObservableCollection<SelectableViewModel>
        {
            new SelectableViewModel
            {
                Code = 'M',
                Name = "Material Design",
                Description = "Material Design in XAML Toolkit",
                Department = Departments[0],
            },
            new SelectableViewModel
            {
                Code = 'D',
                Name = "Dragablz",
                Description = "Dragablz Tab Control",
                Department = Departments[3],
                Food = "Fries"
            },
            new SelectableViewModel
            {
                Code = 'P',
                Name = "Predator",
                Description = "If it bleeds, we can kill it",
                Department = Departments[2],
            }
        };
    }

    public ObservableCollection<Department> Departments { get; } = new();
    public ObservableCollection<Employee> Employees { get; } = new();
    public ObservableCollection<SelectableViewModel> Items1 { get; }
    public ObservableCollection<SelectableViewModel> Items2 { get; }
    public ObservableCollection<SelectableViewModel> Items3 { get; }
    public ObservableCollection<SelectableViewModel> Items4 { get; }

    public IEnumerable<string> Foods => new[] { "Burger", "Fries", "Shake", "Lettuce" };

    public IList<string> Files { get; }

    public IEnumerable<DataGridSelectionUnit> SelectionUnits => new[] { DataGridSelectionUnit.FullRow, DataGridSelectionUnit.Cell, DataGridSelectionUnit.CellOrRowHeader };
}

public class Employee
{
    public Employee(int id, string firstName, string lastName, int age, Department department, IReadOnlyList<Department> departments)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Age = age;
        this.Department = department;
        Departments = departments;
    }

    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public Department Department { get; set; }

    public IReadOnlyList<Department> Departments { get; }
}

public class Department
{
    public Department(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Sets property if it does not equal existing value. Notifies listeners if change occurs.
    /// </summary>
    /// <typeparam name="T">Type of property.</typeparam>
    /// <param name="member">The property's backing field.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">Name of the property used to notify listeners.  This
    /// value is optional and can be provided automatically when invoked from compilers
    /// that support <see cref="CallerMemberNameAttribute"/>.</param>
    protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(member, value))
        {
            return false;
        }

        member = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Notifies listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">Name of the property, used to notify listeners.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public class SelectableViewModel : ViewModelBase
{
    private bool _isSelected;
    private string? _name;
    private string? _description;
    private char _code;
    private double _numeric;
    private string? _food;
    private string? _files;
    private VehicleType _vehicleType;

    public Department Department { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public char Code
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public double Numeric
    {
        get => _numeric;
        set => SetProperty(ref _numeric, value);
    }

    public string? Food
    {
        get => _food;
        set => SetProperty(ref _food, value);
    }

    public string? Files
    {
        get => _files;
        set => SetProperty(ref _files, value);
    }

    public VehicleType VehicleType
    {
        get => _vehicleType;
        set => SetProperty(ref _vehicleType, value);
    }
}

public enum VehicleType
{
    Car,
    Bus,
    Motorcycle,
    Van,
    Scooter,
    Truck
}