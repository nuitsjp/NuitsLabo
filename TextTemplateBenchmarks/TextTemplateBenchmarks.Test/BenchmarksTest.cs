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
    }
}