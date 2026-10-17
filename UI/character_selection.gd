extends Control


func _on_creative_mode_pressed() -> void:
	get_tree().change_scene_to_file("res://maps/scenes/terrain_example_map.tscn")
