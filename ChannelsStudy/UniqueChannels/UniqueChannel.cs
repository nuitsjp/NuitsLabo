using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace UniqueChannels
{
    public static class UniqueChannel
    {
        public static UniqueChannel<T> CreateUnbounded<T>(Func<T, Task> consume, int degreeOfParallelism)
        {
            return new UniqueChannel<T>(Channel.CreateUnbounded<T>(), consume, degreeOfParallelism);
        }
    }

    public class UniqueChannel<T>
    {
        private readonly Channel<T> _channel;
        private readonly Func<T, Task> _consume;
        private readonly int _degreeOfParallelism;
        private readonly IEnumerable<Task> _consumers;
        private readonly HashSet<T> _items = new HashSet<T>();

        public UniqueChannel(Channel<T> channel, Func<T, Task> consume, int degreeOfParallelism)
        {
            _channel = channel;
            _consume = consume;
            _degreeOfParallelism = degreeOfParallelism;
            _consumers = Enumerable
                .Range(1, degreeOfParallelism)
                .Select(_ => CreateConsumer())
                .ToList();
        }

        public ValueTask<bool> WaitToReadAsync() => _channel.Reader.WaitToReadAsync();
        
        public bool TryWrite(T item)
        {
            lock (_items)
            {
                if(_items.Contains(item)) return false;

                if (!_channel.Writer.TryWrite(item)) return false;

                _items.Add(item);
                return true;
            }

        }

        public void WriterComplete() => _channel.Writer.Complete();

        public async Task WaitReadCompleted() => await Task.WhenAll(_consumers);

        private Task CreateConsumer() =>
            Task.Run(async () =>
            {
                while (await _channel.Reader.WaitToReadAsync())
                {
                    if (_channel.Reader.TryRead(out var item))
                    {
                        try
                        {
                            await _consume(item);
                        }
                        finally
                        {
                            lock (_items)
                            {
                                _items.Remove(item);
                            }
                        }
                    }
                }
            });

    }
}