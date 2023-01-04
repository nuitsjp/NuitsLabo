namespace GcSpreadWithValueObject;

public class Item
{
    public Item(string name, Yen price)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; }
    public Yen Price { get; }
}