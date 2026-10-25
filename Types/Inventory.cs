using System;
using System.Collections.Generic;

namespace UAM3D.Types;

/// <summary>
/// Class for the Players' inventory, in which <see cref="Item">Items</see> are stored.
/// </summary>
public class Inventory
{
	private List<InventorySlot> Slots { get; }

	/// <summary>
	/// Creates an inventory with the specified number of empty slots.
	/// </summary>
	/// <param name="size">The number of slots to create.</param>
	public Inventory(int size)
	{
		Slots = [];
		for (var i = 0; i < size; i++)
		{
			this.AddSlot(new InventorySlot());
		}
	}

	/// <summary>
	/// Adds a slot to the end of the inventory.
	/// </summary>
	/// <param name="slot">The slot to add.</param>
	private void AddSlot(InventorySlot slot) => Slots.Add(slot);


	/// <summary>
	/// Adds a quantity of an item to the inventory, filling compatible stacks first.
	/// Any quantity that cannot fit is passed to <see cref="DropItem"/>.
	/// </summary>
	/// <param name="item">The item to add.</param>
	/// <param name="amount">The number of units to add. Defaults to 1.</param>
	public void AddItem(Item item, int amount = 1)
	{
		List<int> allowedSlotIndexes = [];
		foreach (var slot in Slots)
		{
			if (slot.item == item || slot.item == null) allowedSlotIndexes.Add(Slots.IndexOf(slot));
		}

		for (var i = 0; amount > 0; i++)
		{
			if (allowedSlotIndexes.Count == 0)
			{
				DropItem(item, amount);
				return;
			}

			
			var slot = Slots[allowedSlotIndexes[i]];
			int amountToAdd 
				= slot.amount + amount > slot.item.maxStackSize 
				  && slot.item.maxStackSize != -1
					? slot.item.maxStackSize - slot.amount 
					: amount;
			
			Slots[allowedSlotIndexes[i]].amount += amountToAdd;
			Slots[allowedSlotIndexes[i]].item = item;
			Slots.RemoveAt(allowedSlotIndexes[i]);
			amount -= amountToAdd;
		}
	}
	
	/// <summary>
	/// Removes a quantity of an item from the inventory.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	/// <param name="amount">The number of units to remove. Defaults to 1.</param>
	/// <exception cref="Exception">Thrown if the inventory does not contain enough of the item.</exception>
	public void RemoveItem(Item item, int amount = 1)
	{
		int totalItems = 0;
		List<int> slotIndexes = [];
		foreach (var slot in Slots)
		{
			if (slot.item != item) continue;
			slotIndexes.Add(Slots.IndexOf(slot));
			totalItems+= slot.amount;
		}

		if (amount < totalItems) throw new Exception($"Not enough item \"{item.id}\" in inventory");
		for (var i = 0; amount > 0; i++)
		{
			int amountToRemove = Slots[slotIndexes[i]].amount >= amount
				? amount
				: Slots[slotIndexes[i]].amount;

			Slots[slotIndexes[i]].amount -= amountToRemove;
			amount -= amountToRemove;
		}
	}

    /// <summary>
    /// Removes all items from a given slot.
    /// </summary>
    /// <param name="slot">The slot from which to remove the items.</param>
    public void RemoveFromSlot(InventorySlot slot)
    {
        slot.item = null;
        slot.amount = new int();
    }

	
	/// <summary>
	/// Drops a quantity of an item that cannot fit in the inventory.
	/// </summary>
	/// <param name="item">The item to drop.</param>
	/// <param name="amount">The number of units to drop. Defaults to 1.</param>
	/// <remarks>This method is currently not implemented.</remarks>
	/// <exception cref="NotImplementedException">Always thrown because dropping is not implemented.</exception>
	[Obsolete("Not yet implemented")]
	public void DropItem(Item item, int amount = 1) => throw new NotImplementedException();
	// TODO: IMPLEMENT
}

/// <summary>
/// Represents an item stack held in an <see cref="Inventory"/>.
/// </summary>
public class InventorySlot
{
	/// <summary>
	/// The item in this slot, or <see langword="null"/> when the slot is empty.
	/// </summary>
	public Item item;

	/// <summary>
	/// The number of items in the stack.
	/// </summary>
	public int amount;
}
