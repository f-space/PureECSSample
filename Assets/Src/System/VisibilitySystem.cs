using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(RenderSystem))]
public class VisibilitySystem : SystemBase
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		Entities
			.WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
			.ForEach((Entity entity, in VisibleWhile target) =>
			{
				EntityManager.SetEnabled(entity, game.Phase == target.Phase);
			})
			.WithStructuralChanges()
			.Run();
	}
}