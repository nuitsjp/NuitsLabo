using System;
using System.Net.Http;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.Http.HttpClients;

namespace SerilogStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ready to Enter.");
            Console.ReadLine();

            ILogger log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Http(
                    requestUri: "http://localhost:5000/Logs", 
                    queueLimitBytes: null,
                    httpClient: new JsonHttpClient(new HttpClient(new HttpClientHandler { UseDefaultCredentials = true })))
                .WriteTo.Console()
                .CreateLogger();

            log.Information("Logging {@Heartbeat} from {Computer}", "heartbeat", "computer");

            Console.WriteLine("Completed.");
            Console.ReadLine();
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
