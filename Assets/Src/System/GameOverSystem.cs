using Unity.Collections;
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
		using (NativeArray<GameStateChangedEvent> events = this.eventQuery.ToComponentDataArray<GameStateChangedEvent>(Allocator.TempJob))
		{
			foreach (GameStateChangedEvent ev in events)
			{
				if (ev.NextState == GameState.GameOver)
				{
					EntityManager.AddComponent(this.ballQuery, typeof(Frozen));
				}
			}
		}
	}
}