using System;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FluentAssertionsStudy
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using (var _ = new AssertionScope())
            {
                var foo = "foo";
                foo
                    .Should().Be("bar")
                    .And.BeEmpty();
            }
        }

        [Fact]
        public void Test2()
        {
            var foo = "foo";
            foo
                .Should().Be("bar")
                .And.BeEmpty();
        }
    }
}
