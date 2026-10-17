extends Node3D

@export var worldSize: Vector3 = Vector3(32, 32, 32)
@export_range(-1, 1) var cutoff: float = 0.1
@onready var cube := $testCube
@onready var multiMeshInstance3D:= $terrainGenerator

var count:= 0
var data: Array[Vector3] = []

func _ready() -> void:
	var noise = FastNoiseLite.new()
	#noise.frequency = 0.05
	for x in range(worldSize.x):
		for y in range(worldSize.y):
			for z in range(worldSize.z):
				var random = noise.get_noise_3d(x, y, z)
				if random >= cutoff:
					data.append(Vector3(x, y, z))
	remove_child(cube)
	multiMeshInstance3D.multimesh.instance_count = data.size()
	for i in range(multiMeshInstance3D.multimesh.instance_count):
		multiMeshInstance3D.multimesh.set_instance_transform(i, Transform3D(Basis(), data[i]))
