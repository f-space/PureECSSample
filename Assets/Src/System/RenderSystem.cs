using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[AlwaysUpdateSystem]
public class RenderSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		if (UnityEngine.Rendering.RenderPipelineManager.currentPipeline is ECSRenderPipeline render)
		{
			render.Queue.Clear();

			Entities.ForEach((Visual visual, ref LocalToWorld world) =>
			{
				render.Queue.Add(new RenderRequest
				{
					Mesh = visual.Mesh,
					Material = visual.Material,
					WorldMatrix = world.Value,
				});
			});
		}
	}
}