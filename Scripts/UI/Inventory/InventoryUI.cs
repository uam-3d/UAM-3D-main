using System.Collections.Generic;
using Godot;

namespace UAM3D.Scripts.UI.Inventory;

public partial class InventoryUI : Control
{
	[Signal] public delegate void InventoryOpenedEventHandler();
	
	private List<UISlot> _slots = new();
	private PackedScene _inventorySlot = GD.Load<PackedScene>("res://Scenes/UI/Inventory/inventory_slot.tscn");
	private GridContainer _inventoryContainer;
	public Types.Inventory inventory;
	private CanvasLayer _parentNode;
	private Node _gameManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_inventoryContainer = GetNode<GridContainer>("InventoryContainer/GridContainer");
		_parentNode = GetNode<CanvasLayer>("../");
		_gameManager = GetNode<Node>("/root/GameManager");
		
		CloseInventory();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("tab"))
		{
			_gameManager.Set("inInventoryMenu", !_gameManager.Get("inInventoryMenu").AsBool());
			if (_gameManager.Get("inInventoryMenu").AsBool())
			{
				OpenInventory();
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			else
			{
				CloseInventory();
				Input.MouseMode = Input.MouseModeEnum.Captured;
			}
		}
	}

	private void InitInventory(Types.Inventory inv)
	{
		foreach (var slot in inv.GetSlots())
		{
			var instance = (UISlot)_inventorySlot.Instantiate();
			_slots.Add(instance);
			_inventoryContainer.AddChild(instance);
		}
		
		UpdateInventory(inv);
	}

	public void UpdateInventory(Types.Inventory inv)
	{
		inventory = inv;

		if (inv.GetSlots().Count != _slots.Count)
		{
			InitInventory(inv);
			return;
		}

		for (int i = 0; i < inv.GetSlots().Count; i++)
		{
			_slots[i].Configure(this, i);
			_slots[i].UpdateContents(inv.GetSlots()[i]);
		}
	}

	public bool TrySwapSlots(int sourceIndex, int targetIndex)
	{
		if (inventory == null ||
			sourceIndex < 0 || sourceIndex >= inventory.GetSlots().Count ||
			targetIndex < 0 || targetIndex >= inventory.GetSlots().Count ||
			inventory.GetSlots()[sourceIndex].item == null)
		{
			return false;
		}

		inventory.SwapSlots(sourceIndex, targetIndex);
		UpdateInventory(inventory);
		return true;
	}

	public void OpenInventory()
	{
		_parentNode.Show();
		EmitSignal(SignalName.InventoryOpened);
	}

	public void CloseInventory()
	{
		_parentNode.Hide();
	}
}
