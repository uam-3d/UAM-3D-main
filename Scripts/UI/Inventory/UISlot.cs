using Godot;
using UAM3D.Types;

namespace UAM3D.Scripts.UI.Inventory;

public partial class UISlot : Panel
{
	private TextureRect _itemSprite;
	private Label _itemCount;
	private InventoryUI _inventoryUI;
	private int _slotIndex = -1;
	private bool _hasItem;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_itemSprite = GetNode<TextureRect>("ItemSprite");
		_itemCount = GetNode<Label>("ItemSprite/Amount");
		_itemSprite.MouseFilter = MouseFilterEnum.Ignore;
		_itemCount.MouseFilter = MouseFilterEnum.Ignore;
	}

	public void Configure(InventoryUI inventoryUI, int slotIndex)
	{
		_inventoryUI = inventoryUI;
		_slotIndex = slotIndex;
	}
	
	public void UpdateContents(InventorySlot slot)
	{
		Texture2D texture;
		string count;
		string tooltipText;
		
		if (slot.item == null)
		{
			texture = null;
			count = "";
			tooltipText = "";
		}
		else
		{
			texture =  GD.Load<Texture2D>(slot.item.texturePath);
			count = slot.amount.ToString("D");
			tooltipText = slot.item.description;
		}

		_itemSprite.Texture = texture;
		_itemCount.Text = count;
		TooltipText = tooltipText;
		_hasItem = slot.item != null;
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (!_hasItem || _inventoryUI == null || _slotIndex < 0)
		{
			return default;
		}

		var preview = new TextureRect
		{
			Texture = _itemSprite.Texture,
			CustomMinimumSize = new Vector2(130, 130),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
			MouseFilter = MouseFilterEnum.Ignore
		};
		SetDragPreview(preview);
		return Variant.From(_slotIndex);
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return _inventoryUI != null &&
			data.VariantType == Variant.Type.Int &&
			data.AsInt32() >= 0 &&
			data.AsInt32() != _slotIndex;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (_CanDropData(atPosition, data))
		{
			_inventoryUI.TrySwapSlots(data.AsInt32(), _slotIndex);
		}
	}
}
