using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UniqueChannels.Test
{
    public class UniqueChannelsTest
    {
        private readonly ITestOutputHelper _output;

        public UniqueChannelsTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(Timeout = 1_000)]
        public async Task Basic()
        {
            var alarm = new Alarm();

            var readEmployees = new List<Employee>();
            var consume = new Func<Employee, Task>(employee =>
            {
                alarm.Sleep(() =>
                {
                    readEmployees.Add(employee);
                });
                return Task.CompletedTask;
            });
            var channel = UniqueChannel.CreateUnbounded<Employee>(consume, 1);

            var employee1 = new Employee(1);
            var employee2 = new Employee(2);
            var employee3 = new Employee(3);
            var employee1Other = new Employee(1);

            Assert.True(channel.TryWrite(employee1));
            Assert.True(channel.TryWrite(employee2));
            // employee1がまだ消費されていないのでemployee1Otherを追加できない
            Assert.False(channel.TryWrite(employee1Other));
            Assert.Empty(readEmployees);

            // employee1を消費しemployee1Otherが追加できるようになる
            alarm.Wakeup();
            Assert.Single(readEmployees);
            Assert.True(channel.TryWrite(employee1Other));
            
            alarm.Wakeup();
            Assert.Equal(2, readEmployees.Count);
            Assert.Equal(employee2, readEmployees.Last());

            alarm.Wakeup();
            Assert.Equal(3, readEmployees.Count);
            Assert.Equal(employee1Other, readEmployees.Last());

            _output.WriteLine("11");
            channel.WriterComplete();
            _output.WriteLine("22");
            await channel.WaitReadCompleted();
            _output.WriteLine("32");
        }

        private class Alarm
        {
            private bool IsAwoke { get; set; } = false;
            private bool IsSlept { get; set; } = false;

            public void Sleep(Action action)
            {
                Monitor.Enter(this);
                try
                {
                    IsSlept = true;
                    Monitor.PulseAll(this);
                    // モニター
                    Monitor.Wait(this);
                    action();
                    IsAwoke = true;
                    Monitor.PulseAll(this);
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }

            public void Wakeup()
            {
                Monitor.Enter(this);
                try
                {
                    // まず寝るまで待つ
                    while (!IsSlept)
                    {
                        Monitor.Wait(this);
                    }

                    // 起こす
                    Monitor.PulseAll(this);

                    // 起きるまでループして待つ
                    while (!IsAwoke)
                    {
                        Monitor.Wait(this);
                    }

                    // リセット
                    IsSlept = false;
                    IsAwoke = false;
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }
        }

        public class Employee
        {
            private readonly int _id;

            public Employee(int id)
            {
                _id = id;
            }

            public static bool operator ==(Employee a, Employee b) => Equals(a, b);

            public static bool operator !=(Employee a, Employee b) => !Equals(a, b);

            private bool Equals(Employee other)
            {
                return _id == other._id;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Employee) obj);
            }

            public override int GetHashCode()
            {
                return _id;
            }
        }
    }
}