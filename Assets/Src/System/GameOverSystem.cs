using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(CollisionGroup))]
[UpdateAfter(typeof(CollisionSystem))]
public class GameOverSystem : ComponentSystem
{
	private ComponentGroup group;

	protected override void OnCreateManager()
	{
		this.group = GetComponentGroup(ComponentType.ReadOnly<Ball>(), ComponentType.ReadOnly<Position>());
	}

	protected override void OnUpdate()
	{
		ArchetypeChunkComponentType<Position> positionType = GetArchetypeChunkComponentType<Position>(true);

		using (NativeArray<ArchetypeChunk> chunks = this.group.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				NativeArray<Position> positions = chunk.GetNativeArray(positionType);
				foreach (Position position in positions)
				{
					if (position.Y < 0f)
					{
						Entity entity = EntityManager.CreateEntity(System.Array.Empty<ComponentType>());
						EntityManager.AddComponentData(entity, new GameStateChangedEvent { NextState = GameState.GameOver });
						return;
					}
				}
			}
		}
	}
}