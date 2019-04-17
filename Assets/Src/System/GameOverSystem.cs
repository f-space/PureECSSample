using Unity.Entities;

[UpdateInGroup(typeof(EventHandlingSystemGroup))]
public class GameOverSystem : ComponentSystem
{
	private EntityQuery eventQuery;

	private EntityQuery ballQuery;

	protected override void OnCreate()
	{
		this.eventQuery = GetEntityQuery(ComponentType.ReadOnly<GameStateChangedEvent>());
		this.ballQuery = GetEntityQuery(new EntityQueryDesc
		{
			All = new[] { ComponentType.ReadOnly<Ball>() },
			None = new[] { ComponentType.ReadWrite<Frozen>() },
		});

		RequireForUpdate(this.eventQuery);
	}

	protected override void OnUpdate()
	{
		this.ForEach((in EntityManager manager, ref GameStateChangedEvent ev) =>
		{
			if (ev.NextState == GameState.GameOver)
			{
				manager.AddComponent(this.ballQuery, typeof(Frozen));
			}
		}, EntityManager, this.eventQuery);
	}
}