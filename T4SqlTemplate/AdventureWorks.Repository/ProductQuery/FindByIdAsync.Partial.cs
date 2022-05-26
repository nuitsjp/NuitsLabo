namespace AdventureWorks.Repository.ProductQuery;

public partial class FindByIdAsync
{
    public FindByIdAsync(ProductId id)
    {
        Id = id;
    }

    public ProductId Id { get; }
}