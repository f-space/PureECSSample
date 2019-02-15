using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DynamicTextMeshBuilder
{
	public readonly Font Font;
	public readonly StringBuilder Text = new StringBuilder();
	public readonly Mesh Mesh = new Mesh();
	private readonly List<Vector3> vertices = new List<Vector3>();
	private readonly List<Vector2> uvs = new List<Vector2>();
	private readonly List<int> triangles = new List<int>();

	public DynamicTextMeshBuilder(Font font)
	{
		this.Font = font;
		this.Mesh.MarkDynamic();
	}

	public void Build()
	{
		ResourceUtility.BuildTextMesh(Font, Text, vertices, uvs, triangles);

		Mesh.Clear();
		Mesh.SetVertices(vertices);
		Mesh.SetUVs(0, uvs);
		Mesh.SetTriangles(triangles, 0);
	}
}