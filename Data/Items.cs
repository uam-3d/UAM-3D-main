using System.Collections.Generic;
using UAM3D.Types;

namespace UAM3D.Data;

/// <summary>
/// All items are stored here
/// </summary>
public static class Items
{
	public static readonly Item TestItem = new()
	{
		id = "testItem",
		name = "Test Item", 
		description = "A test Item.",
		itemClass = ItemClass.Unknown,
		maxStackSize = (int)StackSize.None,
		rarity = Rarity.None,
		texturePath = "null",
	};

	public static readonly Item Stick = new ()
	{
		id = "stick",
		name = "Stick",
		description = "A stick from a tree",
		itemClass = ItemClass.Unknown,
		maxStackSize = (int)StackSize.Large,
		rarity = Rarity.Common,
		texturePath = "res://temporary sprites/stick.png",
	};

	public static readonly Item OakWood = new()
	{
		id = "oakWood",
		name = "Oak Wood",
		description = "wood",
		itemClass = ItemClass.Unknown,
		maxStackSize = (int)StackSize.Large,
		rarity = Rarity.Common,
		texturePath = "res://temporary sprites/oakWood.png",
	};

	public static readonly Item WoodenGun = new()
	{
		id = "woodenGun",
		name = "Wooden Gun",
		description = "a wooden gun used to kill enemies. duh.",
		itemClass = ItemClass.Weapon,
		maxStackSize = (int)StackSize.Single,
		rarity = Rarity.Common,
		texturePath = "res://temporary sprites/wooden gun.png",
	};

	public static readonly Item DefaultGun = new()
	{
		id = "defaultGun",
		name = "Default Gun",
		description = "woah marmi, where did you get that from?",
		itemClass = ItemClass.Weapon,
		maxStackSize = (int)StackSize.Single,
		rarity = Rarity.Common,
		texturePath = "res://icon.svg",
	};

	public static readonly Item IronBar = new()
	{
		id = "ironBar",
		name = "Iron Bar",
		description = "A clump of an element of the atomic number 26.",
		itemClass = ItemClass.Unknown,
		maxStackSize = (int)StackSize.Medium,
		rarity = Rarity.Common,
		texturePath = "null",
	};

	public static readonly Item Bullet = new()
	{
		id = "bullet",
		name = "Bullet",
		description = "A bullet. At high speeds, it can turn a living person into a non-living person.",
		itemClass = ItemClass.Consumable,
		maxStackSize = (int)StackSize.VeryLarge,
		rarity = Rarity.Common,
		texturePath = "null",
	};
}
