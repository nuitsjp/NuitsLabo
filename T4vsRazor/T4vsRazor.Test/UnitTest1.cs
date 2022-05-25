using FluentAssertions;
using Scriban;
using Xunit;

namespace T4vsRazor.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Razor()
        {
            T4VsRazorBench bench = new();
            var actual = bench.Razor();
            Assert.Equal(@"
select
	*
from
	Employee
where
	1 = 1
and FirstNameLastName", actual);
        }

        [Fact]
        public void Scriban()
        {
            T4VsRazorBench bench = new();
            var actual = bench.ScribanRender();
            Assert.Equal(@"
select
	*
from
	Employee
where
	1 = 1

	and FirstName = FirstName


	and LastName = LastName

", actual);
        }

        [Fact]
        public void ScribanWithParse()
        {
            T4VsRazorBench bench = new();
            var actual = bench.ScribanWithParse();
            Assert.Equal(@"
select
	*
from
	Employee
where
	1 = 1

	and FirstName = FirstName


	and LastName = LastName

", actual);
        }

        [Fact]
        public void Handlebars()
        {
            T4VsRazorBench bench = new();
            var actual = bench.HandlebarsInvoke();
            Assert.Equal(@"
select
	*
from
	Employee
where
	1 = 1
	and FirstName = FirstName
	and LastName = LastName
", actual);
        }

        [Fact]
        public void HandlebarsWithParse()
        {
            T4VsRazorBench bench = new();
            var actual = bench.HandlebarsWithParse();
            Assert.Equal(@"
select
	*
from
	Employee
where
	1 = 1
	and FirstName = FirstName
	and LastName = LastName
", actual);
        }

        [Fact]
        public void DotLiquid()
        {
            T4VsRazorBench bench = new();

            var actual = bench.DotLiquid();
            Assert.Equal(@"
select
	*
from
	Employy
where
	1 = 1

	and FirstName = FirstName


	and LastName = LastName

", actual);
        }

        [Fact]
        public void ScribanTest()
        {
            Template.Parse("Hello {{firstname}}!").Render(new {Firstname = "World"})
                .Should().Be("Hello World!");
            Template.Parse("Hello {{firstName}}!").Render(new { FirstName = "World" })
                .Should().Be("Hello !");
            Template.Parse("Hello {{firstName}}!").Render(new { firstName = "World" })
                .Should().Be("Hello !");
            Template.Parse("Hello {{firstName}}!").Render(new { FirstName = "World" })
                .Should().Be("Hello !");
            Template.Parse("Hello {{FirstName}}!").Render(new { FirstName = "World" })
                .Should().Be("Hello !");
        }
    }
}