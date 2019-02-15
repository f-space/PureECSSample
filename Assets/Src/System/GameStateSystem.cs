using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(EventHandlingGroup))]
public class GameStateSystem : ComponentSystem
{
	private ComponentGroup group;

	protected override void OnCreateManager()
	{
		this.group = GetComponentGroup(ComponentType.Create<GameStateChangedEvent>());
	}

	protected override void OnUpdate()
	{
		ArchetypeChunkEntityType entityType = GetArchetypeChunkEntityType();
		ArchetypeChunkComponentType<GameStateChangedEvent> eventType = GetArchetypeChunkComponentType<GameStateChangedEvent>(true);

		using (NativeArray<ArchetypeChunk> chunks = this.group.CreateArchetypeChunkArray(Allocator.TempJob))
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