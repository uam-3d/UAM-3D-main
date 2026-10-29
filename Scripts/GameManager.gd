extends Node

#region UI
var inMenu: bool = false

#pause menu
var inPauseMenu: bool = false

#seed menu
var CustomSeed: Variant

#inventory menu
var inInventoryMenu: bool = false
#endregion 

#region World generation

const DIMENSION := Vector3(32, 64, 32)

#endregion 

### REMOVE THIS ONCE GAME MANAGER IS CLEANED UP
func _process(_delta: float) -> void:
	if (inPauseMenu or inInventoryMenu):
		inMenu = true
	else:
		inMenu = false
