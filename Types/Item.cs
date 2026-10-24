namespace UAM3D.Types;

/// <summary>
/// //TODO: SUMMARIZE
/// </summary>
public class Item
{
    /// <summary>
    /// The ID of the item
    /// </summary>
    public string Id;
    /// <summary>
    /// The display name of the item
    /// </summary>
    public string Name;
    /// <summary>
    /// The Class of the Item
    /// </summary>
    public ItemClass ItemClass;
    /// <summary>
    /// The max stack size, unlimited is -1
    /// </summary>
    public int MaxStackSize;
    /// <summary>
    /// Description for the Item
    /// </summary>
    public string Description;
    /// <summary>
    /// The Item Rarity
    /// </summary>
    public Rarity Rarity;
    /// <summary>
    /// The Path for the Item Texture
    /// </summary>
    public string TexturePath;
}