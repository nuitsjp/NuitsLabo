// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");



IEnumerable<Item> GetLowPriceItems(IEnumerable<Item> items)
{
    return items
        .Where(item => item.Price <= 100);
}


public class Item
{
    public int Price { get; set; }
}