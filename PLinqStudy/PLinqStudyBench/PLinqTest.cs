using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace PLinqStudyBench
{
    public class PLinqTest
    {
        // [Benchmark]
        // public void Sequential()
        // {
        //     var bag = new ConcurrentBag<int>();
        //     var exceptionBag = new ConcurrentBag<Exception>();
        //
        //     Enumerable
        //         .Range(0, 100)
        //         .ToList()
        //         .ForEach(x =>
        //         {
        //             try
        //             {
        //                 Task.Delay(TimeSpan.FromMilliseconds(10)).Wait();
        //                 bag.Add(x);
        //                 if (x % 2 == 1)
        //                 {
        //                     throw new Exception(x.ToString());
        //                 }
        //             }
        //             catch (Exception e)
        //             {
        //                 exceptionBag.Add(e);
        //             }
        //         });
        // }
        
        [Benchmark]
        public void Parallel()
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
                            throw new Exception(x.ToString());
                        }
                    });
                }
                catch (Exception e)
                {
                    exceptionBag.Add(e);
                }
            }).ToArray();
            Task.WhenAll(tasks).Wait();
        }

        [Benchmark]
        public void ParallelWithDegreeOfParallelism()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel()
                .WithDegreeOfParallelism(8);
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
                            throw new Exception(x.ToString());
                        }
                    });
                }
                catch (Exception e)
                {
                    exceptionBag.Add(e);
                }
            }).ToArray();
            Task.WhenAll(tasks).Wait();
        }

        [Benchmark]
        public void ParallelForAll()
        {
            var bag = new ConcurrentBag<int>();
            var exceptionBag = new ConcurrentBag<Exception>();
            
            var parallelQuery = Enumerable
                .Range(0, 100)
                .AsParallel()
                .WithDegreeOfParallelism(8);
            parallelQuery.ForAll(x =>
            {
                try
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                        bag.Add(x);
                        if (x % 2 == 1)
                        {
                            throw new Exception(x.ToString());
                        }
                    }).Wait();
                }
                catch (Exception e)
                {
                    exceptionBag.Add(e);
                }
            });
        }
    }
}