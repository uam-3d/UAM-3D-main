extends GridMap

@export var worldSize := Vector3i(64, 64, 64)
@export_range(-1, 1) var cutoff := 0.1
var data: Array[Vector3i] = []
@onready var rng := RandomNumberGenerator.new()

func _ready() -> void:
	#clear()
	var noise = FastNoiseLite.new()
	if GameManager.CustomSeed:
		noise.seed = GameManager.CustomSeed
	else:
		noise.seed = rng.randi_range(0, 999999999)
	print(noise.seed)
	for x in range(worldSize.x):
		for y in range(worldSize.y):
			for z in range(worldSize.z):
				var random = noise.get_noise_3d(x, y, z)
				if random >= cutoff:
					data.append(Vector3i(x, y, z))
	
	for c in data:
		set_cell_item(c, 1)
