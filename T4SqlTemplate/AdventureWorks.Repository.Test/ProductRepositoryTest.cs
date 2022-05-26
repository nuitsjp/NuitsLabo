using System.Data.SQLite;
using FluentAssertions;

namespace AdventureWorks.Repository.Test
{
    namespace ProductRepositoryTest
    {
        public class SearchAsync : ProductRepositoryTestBase
        {
            [Fact]
            public async Task 名前で検索する()
            {
                ProductName name = new("HL Road Frame - Black, 58");
                var products = await ProductRepository.SearchAsync(name, null);
                products.Should().NotBeNull()
                    .And.HaveCount(1);

                var product = products[0];
                product.Id.Should().Be((ProductId) 680);
                product.Name.Should().Be(name);
            }

            [Fact]
            public async Task CategoryIdで検索する()
            {
                ProductCategoryId categoryId = new(35);
                var products = await ProductRepository.SearchAsync(null, categoryId);
                products.Should().NotBeNull()
                    .And.HaveCount(3);

                var product = products[0];
                product.Id.Should().Be((ProductId)707);
            }
        }

        public class FindByIdAsync : ProductRepositoryTestBase
        {
            [Fact]
            public async Task Idに一致するプロダクトを取得する()
            {
                ProductId id = new(680);
                var product = await ProductRepository.FindByIdAsync(id);
                product.Id.Should().Be(id);
                product.Name.Should().Be((ProductName)"HL Road Frame - Black, 58");

            }

            [Fact]
            public async Task 該当するIDが存在しなかった場合_例外がスローされる()
            {
                ProductId id = new(-1);
                Func<Task> act = () => ProductRepository.FindByIdAsync(id);
                await act.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        public abstract class ProductRepositoryTestBase
        {
            protected IProductRepository ProductRepository { get; }

            protected ProductRepositoryTestBase()
            {
                ProductRepository = new ProductRepository(
                    new SQLiteConnectionStringBuilder
                    {
                        DataSource = "AdventureWorksLT.db"
                    }.ToString());
            }
        }
    }
}