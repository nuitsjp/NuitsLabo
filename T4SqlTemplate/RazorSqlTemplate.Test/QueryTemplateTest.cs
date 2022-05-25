using FluentAssertions;

namespace RazorSqlTemplate.Test
{
    public class QueryTemplateTest
    {
        [Fact]
        public void Test1()
        {
            QueryTemplate template = new();
            template.Query().Should().Be(@"select
    *
from
    Employee
where
    1 = 1
    <a>foo</a>
");
        }
    }
}