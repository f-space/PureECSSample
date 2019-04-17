using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(EventHandlingSystemGroup))]
public class ResetSystem : ComponentSystem
{
	private EntityQuery eventQuery;
	private EntityQuery ballQuery;

	protected override void OnCreate()
	{
		this.eventQuery = GetEntityQuery(ComponentType.ReadOnly<GameStateChangedEvent>());
		this.ballQuery = GetEntityQuery(ComponentType.ReadOnly<Ball>());

		RequireForUpdate(this.eventQuery);
	}

	protected override void OnUpdate()
	{
		using (NativeArray<GameStateChangedEvent> events = this.eventQuery.ToComponentDataArray<GameStateChangedEvent>(Allocator.TempJob))
		{
			foreach (GameStateChangedEvent ev in events)
			{
				if (ev.NextState == GameState.Ready)
				{
					Reset();
					return;
				}
			}
		}
	}

	private void Reset()
	{
		Game game = GetSingleton<Game>();
		game.TotalTime = 0f;
		game.ElapsedTime = 0f;
		game.Score = 0;
		SetSingleton<Game>(game);

		BallGenerator generator = GetSingleton<BallGenerator>();
		generator.NextTime = 0f;
		SetSingleton<BallGenerator>(generator);

		EntityManager.DestroyEntity(this.ballQuery);
	}
}