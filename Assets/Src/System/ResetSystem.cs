using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(EventHandlingGroup))]
public class ResetSystem : ComponentSystem
{
	private ComponentGroup eventGroup;
	private ComponentGroup ballGroup;

	protected override void OnCreateManager()
	{
		this.eventGroup = GetComponentGroup(ComponentType.ReadOnly<GameStateChangedEvent>());
		this.ballGroup = GetComponentGroup(ComponentType.Create<Ball>());
	}

	protected override void OnUpdate()
	{
		ArchetypeChunkComponentType<GameStateChangedEvent> eventType = GetArchetypeChunkComponentType<GameStateChangedEvent>(true);

		using (NativeArray<ArchetypeChunk> chunks = this.eventGroup.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				NativeArray<GameStateChangedEvent> events = chunk.GetNativeArray(eventType);
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

		EntityManager.DestroyEntity(this.ballGroup);
	}
}