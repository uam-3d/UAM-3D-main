using UAM3D.types;

namespace UAM3D.data;

/// <summary>
/// All items are stored here
/// </summary>
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
