namespace UAM3D.Types;

/// <summary>
/// An enumerator for the amount of <see cref="Item">Items</see> that can be held in an <see cref="InventorySlot">Inventory Slot.</see>
/// </summary>
public enum StackSize
{
    None = -1,
    Single = 1,
    Small = 50,
    Medium = 100,
    Large = 200,
    VeryLarge = 500,
}