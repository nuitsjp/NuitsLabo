using FluentAssertions;

namespace TextTemplateBenchmarks.Test
{
    public class BenchmarksTest
    {
        [Fact]
        public void T4()
        {
            new Benchmarks().T4().Should().Be(@"select
	*
from
	Employee
where
	1 = 1
	and FirstName = @FirstName
	and LastName = @LastName
");
        }

        [Fact]
        public void Razor()
        {
            new Benchmarks().Razor().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1
	and FirstName = @FirstName
	and LastName = @LastName
");

        }

        [Fact]
        public void RazorWithCompile()
        {
            new Benchmarks().RazorWithCompile().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1
FirstNameLastName");

        }

        [Fact]
        public void Scriban()
        {
            new Benchmarks().Scriban().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1

	and FirstName = @FirstName


	and LastName = @LastName

");
        }


        [Fact]
        public void ScribanWithParse()
        {
            new Benchmarks().ScribanWithParse().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1

	and FirstName = @FirstName


	and LastName = @LastName

");
        }

        [Fact]
        public void DotLiquid()
        {
            new Benchmarks().DotLiquid().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1

	and FirstName = @FirstName


	and LastName = @LastName

");
        }


        [Fact]
        public void DotLiquidWithParse()
        {
            new Benchmarks().DotLiquidWithParse().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1

	and FirstName = @FirstName


	and LastName = @LastName

");
        }

        [Fact]
        public void Handlebars()
        {
            new Benchmarks().Handlebars().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1
	and FirstName = @FirstName
	and LastName = @LastName
");
        }


        [Fact]
        public void HandlebarsWithCompile()
        {
            new Benchmarks().HandlebarsWithCompile().Should().Be(@"
select
	*
from
	Employee
where
	1 = 1
	and FirstName = @FirstName
	and LastName = @LastName
");
        }
    }
}