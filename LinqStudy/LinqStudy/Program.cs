List<Item> items = new[] { 5, 10, 8, 3, 6, 12 }
    .Select(x => new Item(x))
    .ToList();

var evenItems =
    items
        .Where(x =>
        {
            Console.WriteLine("\"Where\" was invoked.");
            return x.Id % 2 == 0;
        })
        .OrderBy(x => x.Id)
        .ToList();

Console.WriteLine($"Count:{evenItems.Count()}");
foreach (var item in evenItems)
{
    Console.WriteLine(item.Id);
}

class Item
{
    public Item(int id)
    {
        Id = id;
    }

    public int Id { get; }
}