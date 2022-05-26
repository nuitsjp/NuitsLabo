namespace AdventureWorks.Repository.ProductQuery;

public partial class SearchAsync
{
    public SearchAsync(ProductName? name, ProductCategoryId? categoryId)
    {
        Name = name;
        CategoryId = categoryId;
    }

    public ProductName? Name { get; }
    public ProductCategoryId? CategoryId { get; }
}