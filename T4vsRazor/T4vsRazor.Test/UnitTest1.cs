using Xunit;

namespace T4vsRazor.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Razor()
        {
            T4vsRazorBench bench = new();
            var actual = bench.Razor();
            Assert.Equal(@"
select
	*
from
	Employy
where
	1 = 1
FirstNameLastName", actual);
        }

        [Fact]
        public void DotLiquid()
        {
            T4vsRazorBench bench = new();

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
    }
}