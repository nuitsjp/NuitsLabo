using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LinqStudy.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Item[] items = new[] { 5, 10, 8, 3, 6, 12 }
                .Select(x => new Item(x))
                .ToArray();

            var evenNumbers =
                items
                    .Where(x => x.Id % 2 == 0)
                    .ToArray();
        }

        [Fact]
        public void Test2()
        {
            int[] numbers = { 5, 10, 8, 3, 6, 12 };

            List<int> evenNumberList = new();
            foreach (var number in numbers)
            {
                if (number % 2 == 0)
                {
                    evenNumberList.Add(number);
                }
            }

            evenNumberList.Sort((x, y) => x - y);
            var evenNumbers = evenNumberList.ToArray();


            int[] expected = { 6, 8, 10, 12 };
            Assert.Equal(expected, evenNumbers);
        }

        [Fact]
        public void Test3()
        {
            int[] numbers = { 5, 10, 8, 3, 6, 12 };

            var evenNumbers =
                numbers
                    .Where(x => x % 2 == 0)
                    .ToList();
            var orderedEvenNumbers =
                numbers
                    .OrderBy(x => x)
                    .ToList();

            foreach (var number in orderedEvenNumbers)
            {
                Console.WriteLine(number);
            }

        }

        public class Item
        {
            public Item(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        [Fact]
        public void Test4()
        {
            Item[] items = new[] { 5, 10, 8, 3, 6, 12 }
                .Select(x => new Item(x))
                .ToArray();

            var item = items.Single(x => x.Id == 3);
            //var item = items.SingleOrDefault(x => x.Id == 3);
            //var item = items.First(x => x.Id == 3);
            //var item = items.FirstOrDefault(x => x.Id == 3);
        }
    }
}