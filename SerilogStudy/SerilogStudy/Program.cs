using System;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace SerilogStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.Console(new CompactJsonFormatter())
                .WriteTo.Console(
                    LogEventLevel.Information, 
                    outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {ApplicationName} {Message} {LogEvent} {NewLine}{Exception}")
                .CreateLogger();

            //Log.Information("Hello, {param}", new EmployeeId { Value = 100 });
            //Log.Information("Hello, {param}", new Employee(new EmployeeId(100), "Tanaka"));
            Log.Information("Hello, {param}", new ProcessActionId(new ProcessId(new BusinessId(10), 40), 1));

            Log.CloseAndFlush();
        }
    }

    public class EmployeeId
    {
        public int Value { get; }

        public EmployeeId(int value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }

    public class Employee
    {
        public Employee(EmployeeId employeeId, string name)
        {
            EmployeeId = employeeId;
            Name = name;
        }

        public EmployeeId EmployeeId { get; }
        public string Name { get; }

        public override string ToString() => $"{{\"EmployeeId\":{EmployeeId}, \"Name\"={Name}}}";
    }

    public readonly struct BusinessId
    {
        public BusinessId(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override string ToString() => Value.ToString();
    }

    public readonly struct ProcessId
    {
        public ProcessId(BusinessId businessId, int processSno)
        {
            BusinessId = businessId;
            ProcessSno = processSno;
        }

        public BusinessId BusinessId { get; }
        public int ProcessSno { get; }

        public override string ToString() => $"BusinessId:{BusinessId}, ProcessSno={ProcessSno}";
    }

    public readonly struct ProcessActionId
    {
        public ProcessActionId(ProcessId processId, int paSno)
        {
            ProcessId = processId;
            PaSno = paSno;
        }

        public ProcessId ProcessId { get; }
        public int PaSno { get; }

        public override string ToString() =>
            $"{{BusinessId:{ProcessId.BusinessId.Value}, ProcessSno:{ProcessId.ProcessSno}, PaSno:{PaSno}}}";
    }
}
