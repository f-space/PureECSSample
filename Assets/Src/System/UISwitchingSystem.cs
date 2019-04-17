using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(RenderSystem))]
public class UISwitchingSystem : ComponentSystem
{
	private EntityQuery visibleReadyUIQuery, invisibleReadyUIQuery;

	private EntityQuery visibleGameOverUIQuery, invisibleGameOverUIQuery;

	protected override void OnCreate()
	{
		this.visibleReadyUIQuery = GetEntityQuery(ComponentType.ReadOnly<ReadyUI>());
		this.invisibleReadyUIQuery = GetEntityQuery(ComponentType.ReadOnly<ReadyUI>(), ComponentType.ReadOnly<Disabled>());
		this.visibleGameOverUIQuery = GetEntityQuery(ComponentType.ReadOnly<GameOverUI>());
		this.invisibleGameOverUIQuery = GetEntityQuery(ComponentType.ReadOnly<GameOverUI>(), ComponentType.ReadOnly<Disabled>());
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		if (game.State == GameState.Ready)
		{
			EntityManager.RemoveComponent(invisibleReadyUIQuery, typeof(Disabled));
		}
		else
		{
			EntityManager.AddComponent(visibleReadyUIQuery, typeof(Disabled));
		}

		if (game.State == GameState.GameOver)
		{
			EntityManager.RemoveComponent(invisibleGameOverUIQuery, typeof(Disabled));
		}
		else
		{
			EntityManager.AddComponent(visibleGameOverUIQuery, typeof(Disabled));
		}
	}
}