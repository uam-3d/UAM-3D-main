using Godot;
using System;

public partial class Chunk : StaticBody3D
{

	/// <summary>
	/// This is the script for a basic chunk loader. It will handle the world generation. Instead of using cubes and MeshInstances, This script makes use of vertices and SurfaceTools.
	/// </summary>
	
	
	private static readonly Vector3[] Vertices =
	{
		new Vector3(0, 0, 0), // 0
		new Vector3(1, 0, 0), // 1
		new Vector3(0, 1, 0), // 2
		new Vector3(1, 1, 0), // 3
		new Vector3(0, 0, 1), // 4
		new Vector3(1, 0, 1), // 5
		new Vector3(0, 1, 1), // 6
		new Vector3(1, 1, 1), // 7
	};

	private static readonly int[] TOP = [2, 3, 7, 6];
	private static readonly int[] BOTTOM = [0, 4, 5, 1];
	private static readonly int[] LEFT = [6, 4, 0, 2];
	private static readonly int[] RIGHT = [3, 1, 5, 7];
	private static readonly int[] FRONT = [7, 5, 4, 6];
	private static readonly int[] BACK = [2, 0, 1, 3];

	private Array Blocks;

	private SurfaceTool St = new SurfaceTool();
	private ArrayMesh Mesh;
	private MeshInstance3D MeshInstance;

	public Node GameManager;
	public Vector3 Dimension;
	
	public override void _Ready()
	{
		GameManager = GetNode<Node>("/root/GameManager");
		Dimension = GameManager.Get("DIMENSION").AsVector3();
		
		Update();
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
		for (int x = 0; x <= Dimension.X; x++)
		{
			for (int y = 0; y <= Dimension.Y; y++)
			{
				for (int z = 0; z <= Dimension.Z; z++)
				{
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
		Vector3 a = Vertices[i[0]] + Offset; // Moving the vertex at P(0, 1, 0) by the Vector3 Offset (-> the cubes coordinates)
		Vector3 b = Vertices[i[1]] + Offset;
		Vector3 c = Vertices[i[2]] + Offset;
		Vector3 d = Vertices[i[3]] + Offset;
		
		St.AddTriangleFan(([a, b, c]));
		St.AddTriangleFan(([a, c, d]));
	}
}
