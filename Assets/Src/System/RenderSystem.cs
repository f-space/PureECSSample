using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[AlwaysUpdateSystem]
public class RenderSystem : SystemBase
{
	protected override void OnUpdate()
	{
		if (RenderPipelineManager.currentPipeline is ECSRenderPipeline pipeline)
		{
			SetUpCamera(pipeline);
			AddRenderRequests(pipeline);
		}
	}

	private void SetUpCamera(ECSRenderPipeline pipeline)
	{
		Entity entity = GetSingletonEntity<Camera>();
		Camera camera = EntityManager.GetComponentData<Camera>(entity);
		LocalToWorld view = EntityManager.GetComponentData<LocalToWorld>(entity);

		SetViewProjection(pipeline, camera, view);
		SetViewport(pipeline, camera);
	}

	private static void SetViewProjection(ECSRenderPipeline pipeline, in Camera camera, in LocalToWorld view)
	{
		pipeline.ViewMatrix = math.inverse(view.Value);
		pipeline.ProjectionMatrix = float4x4.OrthoOffCenter(
			camera.Left,
			camera.Right,
			camera.Bottom,
			camera.Top,
			camera.Near,
			camera.Far
		);
	}

	private static void SetViewport(ECSRenderPipeline pipeline, in Camera camera)
	{
		float aspectRatio = (camera.Right - camera.Left) / (camera.Top - camera.Bottom);
		float screenWidth = UnityEngine.Screen.width;
		float screenHeight = UnityEngine.Screen.height;

		float width, height;
		if (aspectRatio >= screenWidth / screenHeight)
		{
			width = screenWidth;
			height = screenWidth / aspectRatio;
		}
		else
		{
			width = screenHeight * aspectRatio;
			height = screenHeight;
		}

		pipeline.Viewport = new UnityEngine.Rect(
			(screenWidth - width) / 2f,
			(screenHeight - height) / 2f,
			width,
			height
		);
	}

	private void AddRenderRequests(ECSRenderPipeline pipeline)
	{
		List<RenderRequest> queue = pipeline.Queue;

		queue.Clear();

		Entities
			.ForEach((in LocalToWorld world, in Visual visual) =>
			{
				queue.Add(new RenderRequest
				{
					Mesh = visual.Mesh,
					Material = visual.Material,
					WorldMatrix = world.Value,
				});
			})
			.WithoutBurst()
			.Run();
	}
}