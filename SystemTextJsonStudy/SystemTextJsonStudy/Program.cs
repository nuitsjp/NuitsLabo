using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemTextJsonStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            var employee = (IEmployee)new Employee(100);

            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new InterfaceConverter<IEmployee, Employee>()
                }
            };

            var employeeString = JsonSerializer.Serialize(employee, options);
            Console.WriteLine(employeeString);
            var deserializedUnit = JsonSerializer.Deserialize<IEmployee>(employeeString, options);
        }
    }

    public class InterfaceConverter<TInterface, TImplement> 
        : JsonConverter<TInterface> where TImplement : TInterface
    {
        public override TInterface Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            return (TInterface)JsonSerializer.Deserialize(
                ref reader, typeof(TImplement), options);
        }

        public override void Write(
            Utf8JsonWriter writer, 
            TInterface value, 
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, typeof(TImplement), options);
        }

    }

    public interface IEmployee
    {
        int EmployeeId { get; }
    }

    public class Employee : IEmployee
    {
        public Employee(int employeeId)
        {
            EmployeeId = employeeId;
        }

        public int EmployeeId { get; }
    }
}
