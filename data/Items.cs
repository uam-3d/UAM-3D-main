using UAM3D2.Empty.types;

namespace UAM3D2.Empty.data;

public class Items
{
    public Item TestItem = new Item()
    {
        name = "Test Item",
        id = "TestItem",
        maxStackSize = -1,
        itemClass = ItemClass.Unknown
    };
}