using System.Data.SQLite;
using FluentAssertions;

namespace T4SqlTemplate.Test
{
    public class QueryTemplateTest
    {
        [Fact]
        public async Task QueryAsyncTest()
        {
            var connectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = "AdventureWorksLT.db"
            }.ToString();

            await using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var query = new SearchProductsAsync(null, "AWC Logo");
            var products = (await query.QueryAsync(connection)).ToArray();

            products.Should().NotBeNull()
                .And.HaveCount(1);

            var product = products.Single();
            product.Should().NotBeNull();
            product.ProductID.Should().Be(712l);
            product.Name.Should().Be("AWC Logo Cap");



            connection.Close();
        }
    }
}