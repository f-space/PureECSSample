using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(UpdateSystemGroup))]
public class BallMotionSystem : ComponentSystem
{
	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(new EntityQueryDesc
		{
			All = new[] { ComponentType.ReadOnly<Ball>(), ComponentType.ReadWrite<Position>(), ComponentType.ReadOnly<Velocity>() },
			None = new[] { ComponentType.ReadWrite<Frozen>() },
		});
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		ArchetypeChunkComponentType<Position> positionType = GetArchetypeChunkComponentType<Position>();
		ArchetypeChunkComponentType<Velocity> velocityType = GetArchetypeChunkComponentType<Velocity>(true);

		using (NativeArray<ArchetypeChunk> chunks = this.query.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				NativeArray<Position> positions = chunk.GetNativeArray(positionType);
				NativeArray<Velocity> velocities = chunk.GetNativeArray(velocityType);
				for (int i = 0; i < positions.Length; i++)
				{
					Position position = positions[i];
					position.Y += velocities[i].Y * game.ElapsedTime;
					positions[i] = position;
				}
			}
		}
	}
}