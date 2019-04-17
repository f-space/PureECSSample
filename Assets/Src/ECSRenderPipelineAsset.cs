using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = nameof(ECSRenderPipeline))]
public class ECSRenderPipelineAsset : RenderPipelineAsset
{
	protected override RenderPipeline CreatePipeline()
	{
		return new ECSRenderPipeline();
	}
}

public struct RenderRequest
{
	public Mesh Mesh;
	public Material Material;
	public Vector2 Scale;
	public Vector2 Position;
}

class ECSRenderPipeline : RenderPipeline
{
	private const float AspectRatio = 10f / 16f;
	private const float Width = 5f;
	private const float Height = Width / AspectRatio;
	private const float Baseline = 0.9f;

	private CommandBuffer commands = new CommandBuffer();

	protected override void Render(ScriptableRenderContext context, Camera[] cameras)
	{
		CalculateProjection(out Matrix4x4 projection);
		CalculateViewportRect(out Rect viewport);

		commands.Clear();
		commands.ClearRenderTarget(true, true, Color.black);
		commands.SetViewMatrix(Matrix4x4.identity);
		commands.SetProjectionMatrix(projection);
		commands.SetViewport(viewport);

		World world = World.Active;
		if (world != null)
		{
			RenderSystem system = world.GetExistingSystem<RenderSystem>();

			foreach (RenderRequest request in system.RenderQueue)
			{
				Vector3 position = new Vector3(request.Position.x, request.Position.y, 0f);
				Vector3 scale = new Vector3(request.Scale.x, request.Scale.y, 1f);
				Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
				commands.DrawMesh(request.Mesh, matrix, request.Material);
			}
		}

		context.ExecuteCommandBuffer(commands);
		context.Submit();
	}

	private void CalculateProjection(out Matrix4x4 projection)
	{
		float left = -Width / 2f;
		float right = Width / 2f;
		float bottom = -Height * (1f - Baseline);
		float top = Height * Baseline;

		projection = Matrix4x4.Ortho(left, right, bottom, top, 0f, 1f);
	}

	private void CalculateViewportRect(out Rect rect)
	{
		float height = Screen.height;
		float width = height * AspectRatio;
		float x = (Screen.width - width) / 2f;
		float y = 0;

		rect = new Rect(x, y, width, height);
	}
}