using System;
using Godot;

namespace UAM3D.Scripts.Maps;

/// <summary>
/// This is the script for a basic chunk loader. It will handle the world generation. Instead of using cubes, This script makes use of vertices and SurfaceTools.
/// Each face of each cube is divided into four evenly big triangles. You destroy the blocks by destroying one vertex.
/// </summary>

public partial class Chunk : StaticBody3D
{

	private static readonly Vector3[] Vertices =
	{
		// All outer vertices
		new Vector3(0, 0, 0), // 0
		new Vector3(1, 0, 0), // 1
		new Vector3(0, 1, 0), // 2
		new Vector3(1, 1, 0), // 3
		new Vector3(0, 0, 1), // 4
		new Vector3(1, 0, 1), // 5
		new Vector3(0, 1, 1), // 6
		new Vector3(1, 1, 1), // 7

		// All mid-point vertices
		new Vector3(0.5f, 0.5f, 1), // 8
		new Vector3(0.5f, 0.5f, 0), // 9
		new Vector3(0.5f, 1, 0.5f), // 10
		new Vector3(0.5f, 0, 0.5f), // 11
		new Vector3(1, 0.5f, 0.5f), // 12
		new Vector3(0, 0.5f, 0.5f) // 13
	};

	private static readonly int[] TOP = [2, 3, 7, 6, 10];
	private static readonly int[] BOTTOM = [0, 4, 5, 1, 11];
	private static readonly int[] LEFT = [6, 4, 0, 2, 13];
	private static readonly int[] RIGHT = [3, 1, 5, 7, 12];
	private static readonly int[] FRONT = [7, 5, 4, 6, 8];
	private static readonly int[] BACK = [2, 0, 1, 3, 9];

	private Array Blocks;

	private SurfaceTool St = new SurfaceTool();
	private ArrayMesh Mesh;
	private MeshInstance3D MeshInstance;

	public Node GameManager;
	public Vector3 Dimension;

	public FastNoiseLite Noise;
	public RandomNumberGenerator Rng;

	private float _cutoff = 0.1f;

	public override void _Ready()
	{
		GameManager = GetNode<Node>("/root/GameManager");
		Dimension = GameManager.Get("DIMENSION").AsVector3();
		Rng = new RandomNumberGenerator();
		Noise = new FastNoiseLite();
		if (GameManager.Get("CustomSeed").VariantType != Variant.Type.Nil)
		{
			Noise.Seed = GameManager.Get("CustomSeed").AsInt32();
		}
		else
		{
			Noise.Seed = Rng.RandiRange(-999999999, 999999999);
		}

		Noise.Frequency = 0.02f;
		GD.Print(Noise.Seed);
		Update();
		PolishUp();
	}

	public void Update()
	{
		if (MeshInstance != null) // unload
		{
			MeshInstance.CallDeferred(Node.MethodName.QueueFree);
			MeshInstance = null;
		}

		Mesh = new ArrayMesh();
		MeshInstance = new MeshInstance3D();
		St.Begin(Godot.Mesh.PrimitiveType.Triangles);
		for (int x = 0; x < Dimension.X; x++)
		{
			for (int y = 0; y < Dimension.Y; y++)
			{
				for (int z = 0; z < Dimension.Z; z++)
				{
					if (Noise.GetNoise3D(x, y, z) < _cutoff)
					{
						continue;
					}

					if (z == 0 || Noise.GetNoise3D(x, y, z - 1) < _cutoff)
					{
						NorthFace();
						continue;
					}
					if (z == Dimension.Z - 1 || Noise.GetNoise3D(x, y, z + 1) < _cutoff)
					{
						SouthFace();
						continue;
					}
					if (x == 0 || Noise.GetNoise3D(x - 1, y, z) < _cutoff)
					{
						WestFace();
						continue;
					}
					if (x == Dimension.X - 1 || Noise.GetNoise3D(x + 1, y, z) < _cutoff)
					{
						EastFace();
						continue;
					}
					if (y == 0 || Noise.GetNoise3D(x, y - 1, z) < _cutoff)
					{
						BottomFace();
						continue;
					}
					if (y == Dimension.Y - 1 || Noise.GetNoise3D(x, y + 1, z) < _cutoff)
					{
						TopFace();
						continue;
					}
					CreateBlock(x, y, z);
				}
			}
		}

		St.GenerateNormals(false);
		St.Commit(Mesh);
		MeshInstance.SetMesh(Mesh);
		AddChild(MeshInstance);
		MeshInstance.CreateTrimeshCollision();
	}

	public void CreateBlock(int x, int y, int z)
	{
		CreateFace(TOP, x, y, z);
		CreateFace(BOTTOM, x, y, z);
		CreateFace(LEFT, x, y, z);
		CreateFace(RIGHT, x, y, z);
		CreateFace(FRONT, x, y, z);
		CreateFace(BACK, x, y, z);

	}

	public void CreateFace(int[] i, int x, int y, int z)
	{
		Vector3 Offset = new Vector3(x, y, z);
		Vector3
			a = Vertices[i[0]] +
				Offset; // Moving the vertex at P(0, 1, 0) by the Vector3 Offset (-> the cubes coordinates)
		Vector3 b = Vertices[i[1]] + Offset;
		Vector3 c = Vertices[i[2]] + Offset;
		Vector3 d = Vertices[i[3]] + Offset;
		Vector3 m = Vertices[i[4]] + Offset; // m is the midpoint of the face

		// St.AddTriangleFan(([a, b, c]));
		// St.AddTriangleFan(([a, c, d]));

		St.AddTriangleFan(([a, b, m]));
		St.AddTriangleFan(([d, a, m]));
		St.AddTriangleFan(([b, c, m]));
		St.AddTriangleFan(([c, d, m]));
	}

	public void PolishUp()
	{
		DeleteVertex();
	}

	public void DeleteVertex()
	{

	}

	public void NorthFace()
	{

	}

	public void SouthFace()
	{

	}
	
	public void WestFace()
	{

	}

	public void EastFace()
	{

	}
	
	public void BottomFace()
	{
		
	}    
	
	public void TopFace()
	{

	}
	
}
