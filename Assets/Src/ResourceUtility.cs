using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ResourceUtility
{
	public static Material CreateMaterial(Color color)
	{
		Shader shader = Shader.Find("Unlit/Color");
		Material material = new Material(shader) { color = color };

		return material;
	}

	public static Font CreateAsciiFont(int size)
	{
		Font font = Font.CreateDynamicFontFromOSFont("Times New Roman", size);
		string chars = new string(Enumerable.Range(32, 127 - 32).Select(x => (char)x).ToArray());
		font.RequestCharactersInTexture(chars);

		return font;
	}

	public static Mesh CreateCircle()
	{
		const int Division = 32;
		const float DeltaAngle = Mathf.PI * 2f / Division;

		IEnumerable<int> triangles()
		{
			for (int i = 0; i < Division; i++)
			{
				yield return Division;
				yield return i;
				yield return (i + 1) % Division;
			}
		}

		Mesh mesh = new Mesh();
		mesh.vertices = Enumerable.Range(0, Division)
			.Select(n => new Vector3(-Mathf.Cos(DeltaAngle * n), Mathf.Sin(DeltaAngle * n), 0f) * 0.5f)
			.Concat(new[] { Vector3.zero })
			.ToArray();
		mesh.triangles = triangles().ToArray();

		return mesh;
	}

	public static Mesh CreateQuad()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = new[] {
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(-0.5f, +0.5f, 0f),
			new Vector3(+0.5f, -0.5f, 0f),
			new Vector3(+0.5f, +0.5f, 0f),
		};
		mesh.triangles = new[] { 0, 1, 2, 2, 1, 3 };

		return mesh;
	}

	public static Mesh CreateTextMesh(Font font, string text)
	{
		StringBuilder builder = new StringBuilder(text);
		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> triangles = new List<int>();
		BuildTextMesh(font, builder, vertices, uvs, triangles);

		Mesh mesh = new Mesh();
		mesh.SetVertices(vertices);
		mesh.SetUVs(0, uvs);
		mesh.SetTriangles(triangles, 0);

		return mesh;
	}

	public static Mesh CreateDynamicMesh()
	{
		Mesh mesh = new Mesh();
		mesh.MarkDynamic();
		return mesh;
	}

	public static void BuildTextMesh(Font font, StringBuilder text, List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
	{
		vertices.Capacity = Mathf.Max(vertices.Capacity, text.Length * 4);
		uvs.Capacity = Mathf.Max(uvs.Capacity, text.Length * 4);
		triangles.Capacity = Mathf.Max(triangles.Capacity, text.Length * 6);

		vertices.Clear();
		uvs.Clear();
		triangles.Clear();

		float position = 0f;
		for (int i = 0; i < text.Length; i++)
		{
			if (font.GetCharacterInfo(text[i], out CharacterInfo info))
			{
				vertices.Add(new Vector3(position + info.minX, info.minY, 0f));
				vertices.Add(new Vector3(position + info.minX, info.maxY, 0f));
				vertices.Add(new Vector3(position + info.maxX, info.minY, 0f));
				vertices.Add(new Vector3(position + info.maxX, info.maxY, 0f));

				uvs.Add(info.uvBottomLeft);
				uvs.Add(info.uvTopLeft);
				uvs.Add(info.uvBottomRight);
				uvs.Add(info.uvTopRight);

				triangles.Add(i * 4 + 0);
				triangles.Add(i * 4 + 1);
				triangles.Add(i * 4 + 2);
				triangles.Add(i * 4 + 2);
				triangles.Add(i * 4 + 1);
				triangles.Add(i * 4 + 3);

				position += info.advance;
			}
		}

		Matrix4x4 matrix = Matrix4x4.Scale(Vector3.one / font.lineHeight) * Matrix4x4.Translate(new Vector3(position / -2f, 0f, 0f));
		for (int i = 0; i < vertices.Count; i++)
		{
			vertices[i] = matrix * new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, 1f);
		}
	}
}