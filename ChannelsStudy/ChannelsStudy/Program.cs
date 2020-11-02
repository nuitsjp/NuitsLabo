using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelsStudy
{
    class Program
    {
        private static readonly int ConsumerCount = 3;
        private static readonly int Concurrency = ConsumerCount;
        private static bool _isRunning = true;
        private static readonly Random Random = new Random();
        private static readonly List<int> Queue = new List<int>(Enumerable.Range(0, 20));

        static async Task Main(string[] args)
        {
            //await SingleProducerSingleConsumer();
            //await SingleProducerMultiConsumer();
            //await MultiProducerSingleConsumer();

            var semaphore = new SemaphoreSlim(Concurrency);
            var tasks = Enumerable
                .Range(0, ConsumerCount)
                .Select(x => Task.Run(async () =>
                {
                    while (_isRunning)
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            NewMethod(x);
                        }
                        finally
                        {
                            semaphore.Release();
                            // await Task.Delay(TimeSpan.FromSeconds(_random.Next(2)));
                        }
                    }
                }))
                .ToList();

            Console.ReadLine();

            _isRunning = false;
            
            await Task.WhenAll(tasks);
        }

        private static void NewMethod(int x)
        {
            var item = Take(x);
            try
            {
                Thread.Sleep(item.delay);
                Console.WriteLine($"consumer:{x} item:{item.item} delay:{item.delay}");
            }
            finally
            {
                Add(x, item.item);
                Thread.Sleep(item.delay);
            }
        }

        private static (int item, int delay) Take(int consumer)
        {
            lock (Queue)
            {
                if (true)
                {
                    var item = Queue[0];
                    Queue.RemoveAt(0);
                    var result = (item : item, delay : Random.Next(100, 1000));
                    Console.WriteLine($"Take consumer:{consumer}, item:{result.item}, delay:{result.delay}, _queue:{string.Join(", ", Queue)}");
                    return result;
                }
                else
                {
                    return default;
                }
            }
        }

        private static void Add(int consumer, int item)
        {
            Console.WriteLine($"Add consumer:{consumer}, item:{item}");
            lock (Queue)
            {
                Queue.Add(item);
            }
        }

        private static async Task SingleProducerSingleConsumer()
        {
            var channel = Channel.CreateUnbounded<string>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = true
                });

            var consumer = Task.Run(async () =>
            {
                while (await channel.Reader.WaitToReadAsync())
                {
                    Console.WriteLine(await channel.Reader.ReadAsync());
                }
            });
            var producer = Task.Run(async () =>
            {
                var rnd = new Random();
                for (int i = 0; i < 5; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(rnd.Next(3)));
                    await channel.Writer.WriteAsync($"Message {i}");
                }
                channel.Writer.Complete();
            });

            await Task.WhenAll(producer, consumer);
            Console.WriteLine("Completed.");
        }

        private static async Task SingleProducerMultiConsumer()
        {
            var channel = Channel.CreateUnbounded<string>(
                new UnboundedChannelOptions
                {
                    SingleWriter = true
                });
            
            var consumers = Enumerable
                .Range(1, 3)    // 1～3の数値を取得する
                .Select(consumerNumber =>
                    Task.Run(async () =>
                    {
                        while (await channel.Reader.WaitToReadAsync())
                        {
                            if (channel.Reader.TryRead(out var item))
                            {
                                Console.WriteLine($"Consumer:{consumerNumber} {item}");
                            }
                        }
                    }));
            var producer = Task.Run(async () =>
            {
                var rnd = new Random();
                for (var i = 0; i < 5; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(rnd.Next(3)));
                    channel.Writer.TryWrite($"Message {i}");
                    await channel.Writer.WriteAsync($"Message {i}");
                }
                channel.Writer.Complete();
            });

            await Task.WhenAll(consumers.Union(new[] {producer}));
            Console.WriteLine("Completed.");
        }
        
        private static async Task MultiProducerSingleConsumer()
        {
            var channel = Channel.CreateUnbounded<string>(
                new UnboundedChannelOptions
                {
                    SingleReader = true
                });
            
            var consumer = Task.Run(async () =>
            {
                while (await channel.Reader.WaitToReadAsync())
                {
                    Console.WriteLine(await channel.Reader.ReadAsync());
                }
            });

            var producers = Enumerable
                .Range(1, 3)
                .Select(producerNumber =>Task.Run(async () =>
                {
                    var rnd = new Random();
                    for (var i = 0; i < 5; i++)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(rnd.Next(3)));
//await channel.Writer.WriteAsync($"Producer:{producerNumber} Message {i}");
channel.Writer.TryWrite($"Producer:{producerNumber} Message {i}");
                    }
                    //channel.Writer.TryComplete();
                }));

            await Task.WhenAll(producers);
            channel.Writer.Complete();

            await consumer;
            Console.WriteLine("Completed.");
        }
   }
}