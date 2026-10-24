using UAM3D.Types;

namespace UAM3D.Data;

/// <summary>
/// All items are stored here
/// </summary>
public class Items
{
	public Item TestItem = new Item()
	{
		Name = "Test Item",
		Id = "TestItem",
		MaxStackSize = -1,
		ItemClass = ItemClass.Unknown
	};
}
