using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelsStudy
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await SingleProducerSingleConsumer();
            //await SingleProducerMultiConsumer();
            await MultiProducerSingleConsumer();
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