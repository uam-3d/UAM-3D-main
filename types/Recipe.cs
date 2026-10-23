using System.Collections.Generic;

namespace UAM3D.types;



public class Recipe
{
    public Dictionary<Item, int> inputs;
    public Dictionary<Item, int> outputs;
    public int points;
}