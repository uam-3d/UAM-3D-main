using System.Collections.Generic;

namespace UAM3D.Types;



public class Recipe
{
    public Dictionary<Item, int> Inputs;
    public Dictionary<Item, int> Outputs;
    public int Points;
    
}