using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DynamicTextMeshBuilder
{
	public readonly StringBuilder Text = new StringBuilder();
	private readonly List<Vector3> vertices = new List<Vector3>();
	private readonly List<Vector2> uvs = new List<Vector2>();
	private readonly List<int> triangles = new List<int>();

	public void Build(Mesh mesh, Font font)
	{
		ResourceUtility.BuildTextMesh(font, Text, vertices, uvs, triangles);

		mesh.Clear();
		mesh.SetVertices(vertices);
		mesh.SetUVs(0, uvs);
		mesh.SetTriangles(triangles, 0);
	}
}