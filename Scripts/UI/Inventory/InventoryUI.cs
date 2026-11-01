using System.Collections.Generic;
using Godot;

namespace UAM3D.Scripts.UI.Inventory;

public partial class InventoryUI : Control
{
	[Signal] public delegate void InventoryOpenedEventHandler();
	
	private List<UISlot> _slots = [];
	private PackedScene _inventorySlot = GD.Load<PackedScene>("res://Scenes/UI/Inventory/inventory_slot.tscn");
	private GridContainer _inventoryContainer;
	public Types.Inventory inventory;
	private CanvasLayer _parentNode;
	private Node _gameManager;
	public Panel[] specialSlots = new Panel[7];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_inventoryContainer = GetNode<GridContainer>("Background/Inventory/GridContainer");
		_parentNode = GetNode<CanvasLayer>("../");
		_gameManager = GetNode<Node>("/root/GameManager");

		specialSlots[0] = GetNode<Panel>("Background/PlayerStuff/HandSlot1");
		specialSlots[1] = GetNode<Panel>("Background/PlayerStuff/HandSlot2");
		specialSlots[2] = GetNode<Panel>("Background/PlayerStuff/HandSlot3");

		specialSlots[3] = GetNode<Panel>("Background/PlayerStuff/Head");
		specialSlots[4] = GetNode<Panel>("Background/PlayerStuff/Body");
		specialSlots[5] = GetNode<Panel>("Background/PlayerStuff/Legs");
		specialSlots[6] = GetNode<Panel>("Background/PlayerStuff/Feet");
		CloseInventory();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("tab"))
		{
			if (_gameManager.Get("inPauseMenu").AsBool()) return;
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
