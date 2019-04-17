using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameStateSystem : ComponentSystem
{
	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(typeof(GameStateChangedEvent));
	}

	protected override void OnUpdate()
	{
		ArchetypeChunkEntityType entityType = GetArchetypeChunkEntityType();
		ArchetypeChunkComponentType<GameStateChangedEvent> eventType = GetArchetypeChunkComponentType<GameStateChangedEvent>(true);

		using (NativeArray<ArchetypeChunk> chunks = this.query.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				NativeArray<Entity> entities = chunk.GetNativeArray(entityType);
				NativeArray<GameStateChangedEvent> events = chunk.GetNativeArray(eventType);

				foreach (GameStateChangedEvent ev in events)
				{
					Game game = GetSingleton<Game>();
					game.State = ev.NextState;
					SetSingleton<Game>(game);
				}

				EntityManager.DestroyEntity(entities);
			}
		}
	}
}