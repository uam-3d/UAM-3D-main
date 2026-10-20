using Godot;
using System;
using System.Collections.Generic;
using UAM3D2.Empty.types;

public class Inventory
{
	public List<InventorySlot> slots;

	public Inventory(int size)
	{
		slots = [];
		for (var i = 0; i < size; i++)
		{
			this.AddSlot(new InventorySlot());
		}
	}

	public void AddSlot(InventorySlot slot) => slots.Add(slot);


	/// <summary>
	/// Adds a quantity of a specified item to the Inventory. Drops the rest if items exceed inventory capacity.
	/// </summary>
	/// <param name="item">
	/// The Item type to remove.
	/// </param>
	/// <param name="amount">
	/// The number of units to add. Defaults to 1.
	/// </param>
	public void AddItem(Item item, int amount = 1)
	{
		List<int> allowedSlotIndexes = [];
		foreach (var slot in slots)
		{
			if (slot.item == item || slot.item == null) allowedSlotIndexes.Add(slots.IndexOf(slot));
		}

		for (var i = 0; amount > 0; i++)
		{
			if (allowedSlotIndexes.Count == 0)
			{
				DropItem(item, amount);
				return;
			}

			
			var slot = slots[allowedSlotIndexes[i]];
			int amountToAdd = slot.amount + amount > slot.item.maxStackSize && slot.item.maxStackSize != -1
				? slot.item.maxStackSize - slot.amount 
				: amount;
			
			slots[allowedSlotIndexes[i]].amount += amountToAdd;
			slots[allowedSlotIndexes[i]].item = item;
			slots.RemoveAt(allowedSlotIndexes[i]);
			amount -= amountToAdd;
		}
	}
	
	/// <summary>
	/// Removes a quantity of a specified item from the Inventory.
	/// </summary>
	/// <param name="item">
	/// The item type to remove.
	/// </param>
	/// <param name="amount">
	/// The amount of units to remove. Defaults to 1.
	/// </param>
	/// <exception cref="Exception">
	/// Throws if not enough items of aforementioned type in inventory
	/// </exception>
	public void RemoveItem(Item item, int amount = 1)
	{
		int totalItems = 0;
		List<int> slotIndexes = [];
		foreach (var slot in slots)
		{
			if (slot.item == item) 
			{
				slotIndexes.Add(slots.IndexOf(slot));
				totalItems+= slot.amount;
			}
		}

		if (amount < totalItems) throw new Exception($"Not enough item \"{item.id}\" in inventory");
		for (var i = 0; amount > 0; i++)
		{
			int amountToRemove = slots[slotIndexes[i]].amount >= amount
				? amount
				: slots[slotIndexes[i]].amount;
			
			slots[slotIndexes[i]].amount -= amountToRemove;
			amount -= amountToRemove;
		}
		
	}

	
	/// <summary>
	/// Drops a certain amount of a specified item.
	/// </summary>
	/// <param name="item">
	/// The item to drop
	/// </param>
	/// <param name="amount">
	/// the amount of the item type to drop
	/// </param>
	/// <remarks>This method is currently not implemented</remarks>
	/// <exception cref="NotImplementedException"></exception>
	public void DropItem(Item item, int amount = 1)
	{
		throw new NotImplementedException();
	}
}

public class InventorySlot
{
	public Item item;
	public int amount;
}
