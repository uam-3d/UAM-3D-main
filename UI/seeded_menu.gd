extends Control

@onready var lineEdit := $fgRect/LineEdit

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	lineEdit.text_changed.connect(_on_text_changed)
	lineEdit.max_length = 9

func _on_text_changed(newText: String):
	GameManager.customSeed = int(newText)

func _on_seeded_run_pressed() -> void:
	get_tree().change_scene_to_file("res://scenes/maps/tilemap_test.tscn")

func _on_play_pressed() -> void:
	GameManager.customSeed = null
	get_tree().change_scene_to_file("res://scenes/maps/tilemap_test.tscn")
