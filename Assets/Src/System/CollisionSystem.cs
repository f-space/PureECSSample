using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(CollisionGroup))]
public class CollisionSystem : ComponentSystem
{
	private ComponentGroup group;

	protected override void OnCreateManager()
	{
		this.group = GetComponentGroup(ComponentType.ReadOnly<Ball>(), ComponentType.ReadOnly<Position>(), ComponentType.ReadOnly<Size>());
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		Entity player = GetSingleton<Singleton>().Player;
		Position playerPosition = EntityManager.GetComponentData<Position>(player);
		Size playerSize = EntityManager.GetComponentData<Size>(player);

		ArchetypeChunkEntityType entityType = GetArchetypeChunkEntityType();
		ArchetypeChunkComponentType<Position> positionType = GetArchetypeChunkComponentType<Position>();
		ArchetypeChunkComponentType<Size> sizeType = GetArchetypeChunkComponentType<Size>();

		using (NativeArray<ArchetypeChunk> chunks = this.group.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				NativeArray<Entity> entities = chunk.GetNativeArray(entityType);
				NativeArray<Position> positions = chunk.GetNativeArray(positionType);
				NativeArray<Size> sizes = chunk.GetNativeArray(sizeType);

				for (int i = 0; i < entities.Length; i++)
				{
					Position position = positions[i];
					Size size = sizes[i];
					if (position.X == playerPosition.X && position.Y - size.Height / 2f < playerPosition.Y + playerSize.Height / 2f)
					{
						game.Score += 1;
						PostUpdateCommands.DestroyEntity(entities[i]);
					}
				}
			}
		}

		SetSingleton<Game>(game);
	}
}