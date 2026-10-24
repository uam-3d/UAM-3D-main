using System;
using System.Collections.Generic;
using Godot;
using UAM3D.types;

namespace UAM3D.maps.vertex_worldgen;

public partial class VertexWorldgen : Node3D
{
	public List<Vector3> vertices = new();
	public Vector3 WorldSize = new Vector3(32, 32, 32);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
