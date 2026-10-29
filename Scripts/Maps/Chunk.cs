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
		// new Vector3(0, 0, 0), // 0
		// new Vector3(1, 0, 0), // 1
		// new Vector3(0, 1, 0), // 2
		// new Vector3(1, 1, 0), // 3
		// new Vector3(0, 0, 1), // 4
		// new Vector3(1, 0, 1), // 5
		// new Vector3(0, 1, 1), // 6
		// new Vector3(1, 1, 1), // 7
		new(0, 0, 1),
		new(1, 0, 1),
		new(1, 0, 0),
		new(0, 0, 0),
		new(0, 1, 1),
		new(1, 1, 1),
		new(1, 1, 0),
		new(0, 1, 0),

		// All mid-point vertices
		new Vector3(0.5f, 0.5f, 1), // 8
		new Vector3(0.5f, 0.5f, 0), // 9
		new Vector3(0.5f, 1, 0.5f), // 10
		new Vector3(0.5f, 0, 0.5f), // 11
		new Vector3(1, 0.5f, 0.5f), // 12
		new Vector3(0, 0.5f, 0.5f) // 13
	};

	private static readonly int[][] EdgeCorners =
	{
		new[] { 0, 1 },
		new[] { 1, 2 },
		new[] { 2, 3 },
		new[] { 3, 0 },
		new[] { 4, 5 },
		new[] { 5, 6 },
		new[] { 6, 7 },
		new[] { 7, 4 },
		new[] { 0, 4 },
		new[] { 1, 5 },
		new[] { 2, 6 },
		new[] { 3, 7 }
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
	private int _vertexCount = 0;

	public Vector3 Spawn = new Vector3(0, 0, 0); 

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

		Noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		Noise.Frequency = 0.007f; // smaller Frequency ^= uniform map; higher Frequency is more hilly
		GD.Print(Noise.Seed);
		Update();
	}

	public void Update()
	{
		_vertexCount = 0;
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
					MarchingCube(new Vector3(x, y, z));
				}
			}
		}

		St.GenerateNormals(false);
		St.Commit(Mesh);
		MeshInstance.SetMesh(Mesh);
		AddChild(MeshInstance);
		MeshInstance.CreateTrimeshCollision();
		GD.Print("Vertices: ", _vertexCount);
	}

	// private void MarchingTerrain(Vector3 cellPos)
	// {
	// 	int height = Mathf.RoundToInt((Noise.GetNoise2D(cellPos.X, cellPos.Z) + 1) / 2 * Dimension.Y);
	// 	if (cellPos.Y == height)
	// 	{
	// 		MarchingCube(cellPos);
	// 	}
	//
	// 	if (cellPos.Y < height)
	// 	{
	// 		CreateBlock(
	// 			Mathf.RoundToInt(cellPos.X), Mathf.RoundToInt(cellPos.Y), Mathf.RoundToInt(cellPos.Z));
	// 	}
	// }
	
	private void CreateBlock(int x, int y, int z)
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

		// St.AddTriangleFan(([a, b, m]));
		// St.AddTriangleFan(([d, a, m]));
		// St.AddTriangleFan(([b, c, m]));
		// St.AddTriangleFan(([c, d, m]));
		St.AddTriangleFan(([a, b, c]));
		St.AddTriangleFan(([a, c, d]));
	}

	public void MarchingCube(Vector3 cellPos)
	{
		float height = 0f;
		float[] noiseValue = new float[8];
		int cubeIndex = 0;
		for (int i = 0; i < 8; i++)
		{
			Vector3 position = cellPos + Vertices[i];
			height = (Noise.GetNoise2D(position.X, position.Z) + 1f) / 2f * Dimension.Y;
			noiseValue[i] = height - position.Y + _cutoff;
			if (noiseValue[i] >= _cutoff)
			{
				cubeIndex |= 1 << i;
			}
		}
		int[] triangulation = MarchingCubeTable.Triangulation[cubeIndex];
		for (int i = 0; i + 2 < triangulation.Length && triangulation[i] != -1; i += 3)
		{
			Vector3 a = GetEdgeVertex(
			triangulation[i], cellPos, noiseValue);
			Vector3 b = GetEdgeVertex(
			triangulation[i + 1], cellPos, noiseValue);
			Vector3 c = GetEdgeVertex(
			triangulation[i + 2], cellPos, noiseValue);
			St.AddVertex(a);
			St.AddVertex(b);
			St.AddVertex(c);
			_vertexCount += 3;
		}

		if (Spawn == Vector3.Zero)
		{
			Spawn = new Vector3(
				Dimension.X / 2,
				((Noise.GetNoise2D(Dimension.X / 2, Dimension.Z / 2) + 1f) / 2f * Dimension.Y) + 1,
				Dimension.Z / 2);
			GD.Print(Spawn);
		}
	}

	private Vector3 GetEdgeVertex(
		int edgeIndex, Vector3 cellPos, float[] values)
	{
		int indexA = EdgeCorners[edgeIndex][0];
		int indexB = EdgeCorners[edgeIndex][1];
		Vector3 a = cellPos + Vertices[indexA];
		Vector3 b = cellPos + Vertices[indexB];
		float valueA = values[indexA];
		float valueB = values[indexB];
		float delta = valueB - valueA;
		float t = Mathf.Abs(delta) < Math.Pow(10, -6)
			? 0.5f
			: (_cutoff - valueA) / delta;
		return a.Lerp(b, Mathf.Clamp(t, 0f, 1f));
	}

	public void DeleteVertex()
	{

	}
}
