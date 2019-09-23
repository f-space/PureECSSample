using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(RenderSystem))]
public class VisibilitySystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		Entities.WithIncludeAll().ForEach((Entity entity, ref VisibleWhile target) =>
		{
			if (game.State == target.State)
			{
				EntityManager.RemoveComponent(entity, typeof(Disabled));
			}
			else
			{
				EntityManager.AddComponent(entity, typeof(Disabled));
			}
		});
	}
}