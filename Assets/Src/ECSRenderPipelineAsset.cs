using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = nameof(ECSRenderPipeline))]
public class ECSRenderPipelineAsset : RenderPipelineAsset
{
	protected override RenderPipeline CreatePipeline() => new ECSRenderPipeline();
}

public struct RenderRequest
{
	public Mesh Mesh;
	public Material Material;
	public Matrix4x4 WorldMatrix;
}

public class ECSRenderPipeline : RenderPipeline
{
	private const float AspectRatio = 10f / 16f;
	private const float Width = 5f;
	private const float Height = Width / AspectRatio;
	private const float Baseline = 0.9f;

	public readonly List<RenderRequest> Queue = new List<RenderRequest>();

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

		foreach (RenderRequest request in Queue)
		{
			commands.DrawMesh(request.Mesh, request.WorldMatrix, request.Material);
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