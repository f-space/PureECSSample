using Unity.Entities;

[UpdateInGroup(typeof(UpdateSystemGroup))]
public class BallMotionSystem : SystemBase
{
	protected override void OnUpdate()
	{
		float deltaTime = World.Time.DeltaTime;

		Entities
			.WithAll<Ball, Active>()
			.ForEach((ref Position position, in Velocity velocity) =>
			{
				position.Y += velocity.Y * deltaTime;
			})
			.Schedule();
	}
}