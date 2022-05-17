using System.Text;
using FluentAssertions;

namespace CsvReader.Test
{
    public class CsvReaderTest
    {
        [Fact]
        public void 一番シンプルなケース()
        {
            var input = @"item1,item2
11,12
21,22
";
            var actual = CsvReader.Parse(new StringReader(input)).ToArray();

            actual.Length.Should().Be(2);
            actual[0].Should().BeEquivalentTo("11", "12");
            actual[1].Should().BeEquivalentTo("21", "22");
        }

        [Fact]
        public void ダブルクォーテーションで囲まれている場合()
        {
            var input = @"item1,item2
""11"",""1,""""2""
21,22
";
            var actual = CsvReader.Parse(new StringReader(input)).ToArray();

            actual.Length.Should().Be(2);
            actual[0].Should().BeEquivalentTo("11", "1,\"2");
            actual[1].Should().BeEquivalentTo("21", "22");
        }
    }
}