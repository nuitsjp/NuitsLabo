using System.Data.SQLite;

namespace AdventureWorks.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Product> FindByIdAsync(ProductId id)
        {
            await using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            return await new FindByIdAsync(id).QuerySingleAsync(connection);
        }

        public async Task<IList<Product>> SearchAsync(ProductName? name, ProductCategoryId? categoryId)
        {
            await using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            return (await new SearchAsync(name, categoryId).QueryAsync(connection))
                .ToList();
        }
    }
}