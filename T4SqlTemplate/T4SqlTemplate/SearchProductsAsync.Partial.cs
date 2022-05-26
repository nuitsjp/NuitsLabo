namespace T4SqlTemplate;

public partial class SearchProductsAsync
{
    public SearchProductsAsync(string name)
    {
        Name = $"{name}%";
    }

    public string Name { get; }
}