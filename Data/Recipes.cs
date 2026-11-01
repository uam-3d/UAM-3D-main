using System.Collections.Generic;
using UAM3D.Types;

namespace UAM3D.Data;

public static class Recipes
{
    public static readonly Recipe StickRecipe1 = new()
    {
        inputs = new Dictionary<Item, int>
        {
            { Items.OakWood, 1 }
        },
        outputs = new Dictionary<Item, int>
        {
            { Items.Stick, 4 }
        },
        points = 0
    };

    public static readonly Recipe WoodenGunRecipe1 = new()
    {
        inputs = new Dictionary<Item, int>
        {
            { Items.OakWood, 2 },
            { Items.Stick, 1 },
        },
        outputs = new Dictionary<Item, int>
        {
            { Items.WoodenGun, 1 }
        },
        points = 100,
    };

    public static readonly Recipe BulletRecipe1 = new()
    {
        inputs = new Dictionary<Item, int>
        {
            { Items.IronBar, 1 }
        },
        outputs = new Dictionary<Item, int>
        {
            { Items.Bullet, 16 }
        }
    };
}