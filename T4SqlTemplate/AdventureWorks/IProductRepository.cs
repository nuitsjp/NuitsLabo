namespace AdventureWorks;

public interface IProductRepository
{
    Task<Product> FindByIdAsync(ProductId id);
    Task<IList<Product>> SearchAsync(ProductName? name, ProductCategoryId? categoryId);

}