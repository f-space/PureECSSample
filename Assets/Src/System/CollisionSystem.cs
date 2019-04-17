using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(InteractionSystemGroup))]
public class CollisionSystem : ComponentSystem
{
	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(new EntityQueryDesc
		{
			All = new[] { ComponentType.ReadOnly<Ball>(), ComponentType.ReadOnly<Position>(), ComponentType.ReadOnly<Size>() },
			None = new[] { ComponentType.ReadWrite<Frozen>() },
		});
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		Entity player = GetSingletonEntity<Player>();
		Position playerPosition = EntityManager.GetComponentData<Position>(player);
		Size playerSize = EntityManager.GetComponentData<Size>(player);

		ArchetypeChunkEntityType entityType = GetArchetypeChunkEntityType();
		ArchetypeChunkComponentType<Position> positionType = GetArchetypeChunkComponentType<Position>();
		ArchetypeChunkComponentType<Size> sizeType = GetArchetypeChunkComponentType<Size>();

		bool gameOver = false;
		using (NativeArray<ArchetypeChunk> chunks = this.query.CreateArchetypeChunkArray(Allocator.TempJob))
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
					else if (position.Y < 0f)
					{
						gameOver = true;
					}
				}
			}
		}

		if (gameOver)
		{
			Entity entity = EntityManager.CreateEntity();
			EntityManager.AddComponentData(entity, new GameStateChangedEvent { NextState = GameState.GameOver });
		}

		SetSingleton<Game>(game);
	}
}