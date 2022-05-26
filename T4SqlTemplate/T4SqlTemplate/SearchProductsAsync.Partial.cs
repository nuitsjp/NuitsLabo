namespace T4SqlTemplate;

public partial class SearchProductsAsync
{
    public SearchProductsAsync(long? productId, string? name)
    {
        ProductID = productId;
        Name = $"{name}%";
    }

    // ReSharper disable once InconsistentNaming
    private long? ProductID { get; }
    private string? Name { get; }
}