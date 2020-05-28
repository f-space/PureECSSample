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
	public readonly List<RenderRequest> Queue = new List<RenderRequest>();

	public Matrix4x4 ViewMatrix { get; set;} = Matrix4x4.identity;

	public Matrix4x4 ProjectionMatrix { get; set; } = Matrix4x4.identity;

	public Rect Viewport { get; set; } = Rect.zero;

	private CommandBuffer commands = new CommandBuffer();

	protected override void Render(ScriptableRenderContext context, UnityEngine.Camera[] _)
	{
		commands.Clear();
		commands.ClearRenderTarget(true, true, Color.black);
		commands.SetViewMatrix(ViewMatrix);
		commands.SetProjectionMatrix(ProjectionMatrix);
		commands.SetViewport(Viewport);

		foreach (RenderRequest request in Queue)
		{
			commands.DrawMesh(request.Mesh, request.WorldMatrix, request.Material);
		}

		context.ExecuteCommandBuffer(commands);
		context.Submit();
	}
}