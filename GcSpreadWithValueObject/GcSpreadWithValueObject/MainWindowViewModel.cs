using System.Collections.Generic;

namespace GcSpreadWithValueObject;

public class MainWindowViewModel
{
    public List<Item> Items { get; } =
        new()
        {
            new Item("商品A", new Yen(10000)),
            new Item("商品B", new Yen(2000000))
        };
}