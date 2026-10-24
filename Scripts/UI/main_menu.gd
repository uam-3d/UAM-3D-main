extends Control

@onready var soloPlayText: String = $soloPlay.text
@onready var mainModeText: String = $mainMode.text
@onready var symmetricalModeText: String = $symmetricalMode.text


func _on_main_mode_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/UI/character_selection.tscn")


func _on_solo_play_pressed() -> void:
	$soloPlay.text = "(under \n construction)"


func _on_symmetrical_mode_pressed() -> void:
	$symmetricalMode.text = "(under \n construction)"
