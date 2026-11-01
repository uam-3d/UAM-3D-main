extends Control

@onready var lineEdit := $fgRect/LineEdit
@onready var CharacterSelection := get_parent().get_parent()

func _ready() -> void:
	lineEdit.text_changed.connect(_on_text_changed)
	lineEdit.max_length = 9

func _on_text_changed(newText: String):
	GameManager.CustomSeed = int(newText)

func _on_seeded_run_pressed() -> void:
	if CharacterSelection._unc == true:
		get_tree().change_scene_to_file("res://Scenes/Maps/old_tilemap_test.tscn")
		return
	get_tree().change_scene_to_file("res://Scenes/Maps/tilemap_test.tscn")

func _on_play_pressed() -> void:
	GameManager.CustomSeed = null
	if CharacterSelection._unc == true:
		get_tree().change_scene_to_file("res://Scenes/Maps/old_tilemap_test.tscn")
		return
	get_tree().change_scene_to_file("res://Scenes/Maps/tilemap_test.tscn")

func _on_x_pressed() -> void:
	get_parent().hide()

func _input(_event: InputEvent) -> void:
	if Input.is_action_just_pressed("esc"):
		get_parent().hide()
