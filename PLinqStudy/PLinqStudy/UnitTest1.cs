using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PLinqStudy
{
    public class UnitTest1
    {
        [Fact]
        public void ForAll_And_AggregateException()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel();
            try
            {
                parallelQuery.ForAll(x =>
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(10));
                    bag.Add(x);
                    if (x % 2 == 1)
                    {
                        _client.GetStringAsync("https://redsheeps.com/").Wait();
                    }
                });
            }
            catch (AggregateException e)
            {
                // 中断されるので全部実行されない
                Assert.False(50 == e.InnerExceptions.Count);
            }
            
            // 中断されるので全部実行されない
            Assert.False(100 == bag.Count);
        }

        [Fact]
        public void ForAll_And_TaskWait()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel();
            try
            {
                parallelQuery.ForAll(x =>
                {
                    var task = Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        if (x % 2 == 1)
                        {
                            throw new Exception(x.ToString());
                        }
                    });
                    task.Wait();
                    bag.Add(x);
                    if (!(task.Exception is null))
                    {
                        exceptionBag.Add(task.Exception);
                    }
                });
            }
            catch (AggregateException e)
            {
                // 中断されるので全部実行されない
                Assert.False(50 == e.InnerExceptions.Count);
            }
            
            // 中断されるので全部実行されない
            Assert.False(100 == bag.Count);
        }

        private HttpClient _client = new HttpClient();
        
        [Fact]
        public void ForAll_And_Try_Catch()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel();
            parallelQuery.ForAll(x =>
            {
                try
                {
                    var task = Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        bag.Add(x);
                        if (x % 2 == 1)
                        {
                            await _client.GetStringAsync("https://redsheeps.com/");
                        }
                    });
                    task.Wait();
                }
                catch (Exception e)
                {
                    exceptionBag.Add(e);
                }
            });
            
            Assert.Equal(100, bag.Count);
            Assert.Equal(50, exceptionBag.Count);
        }

        [Fact]
        public async Task ForAll_And_Async_Await_And_Try_Catch()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel();
            parallelQuery.ForAll(async x =>
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        bag.Add(x);
                        if (x % 2 == 1)
                        {
                            await _client.GetStringAsync("https://redsheeps.com/");
                        }
                    });
                }
                catch (Exception e)
                {
                    exceptionBag.Add(e);
                }
            });
            
            // ForAllのasyncは実質待たないのでダメ
            Assert.False(100 == bag.Count);
            Assert.False(50 == exceptionBag.Count);
        }

        [Fact]
        public async Task Select_And_Async_Await()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel();
            var tasks = parallelQuery.Select(async x =>
                {
                    try
                    {
                        await Task.Run(async () =>
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(10));
                            bag.Add(x);
                            if (x % 2 == 1)
                            {
                                await _client.GetStringAsync("https://redsheeps.com/");
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        exceptionBag.Add(e);
                    }
                });
            await Task.WhenAll(tasks);
            
            Assert.Equal(100, bag.Count);
            Assert.Equal(50, exceptionBag.Count);
        }
        
        [Fact]
        public void Select_And_Task_Wait()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel();
            var tasks = parallelQuery.Select(async x =>
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        bag.Add(x);
                        if (x % 2 == 1)
                        {
                            await _client.GetStringAsync("https://redsheeps.com/");
                        }
                    });
                }
                catch (Exception e)
                {
                    exceptionBag.Add(e);
                }
            }).ToArray();
            Task.WhenAll(tasks).Wait();
            
            Assert.Empty(tasks.Where(x => x.Exception != null));
            Assert.Equal(100, bag.Count);
            Assert.Equal(50, exceptionBag.Count);
        }
    }
}