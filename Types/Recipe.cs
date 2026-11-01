using System.Collections.Generic;

namespace UAM3D.Types;

/// <summary>
/// A recipe as a means to acquire a certain <see cref="Item"/>.
/// </summary>
public class Recipe
{
    public Dictionary<Item, int> inputs;
    public Dictionary<Item, int> outputs;
    public int points;
    
}