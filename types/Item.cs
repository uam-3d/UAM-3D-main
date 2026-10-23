namespace UAM3D.types;

/// <summary>
/// A class for an item
/// </summary>
public class Item
{
    /// <summary>
    /// The ID of the item
    /// </summary>
    public string id;
    /// <summary>
    /// The display name of the item
    /// </summary>
    public string name;
    /// <summary>
    /// The Class of the Item
    /// </summary>
    public ItemClass itemClass;
    /// <summary>
    /// The max stack size, unlimited is -1
    /// </summary>
    public int maxStackSize;
    /// <summary>
    /// Description for the Item
    /// </summary>
    public string description;
    /// <summary>
    /// The Item Rarity
    /// </summary>
    public Rarity rarity;
    /// <summary>
    /// The Path for the Item Texture
    /// </summary>
    public string texturePath;
}