using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(UpdateGroup))]
public class BallMotionSystem : ComponentSystem
{
	private ComponentGroup group;

	protected override void OnCreateManager()
	{
		this.group = GetComponentGroup(ComponentType.ReadOnly<Ball>(), ComponentType.ReadOnly<Velocity>(), ComponentType.Create<Position>());
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		ArchetypeChunkComponentType<Position> positionType = GetArchetypeChunkComponentType<Position>();
		ArchetypeChunkComponentType<Velocity> velocityType = GetArchetypeChunkComponentType<Velocity>(true);

		using (NativeArray<ArchetypeChunk> chunks = this.group.CreateArchetypeChunkArray(Allocator.TempJob))
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