using Unity.Entities;

[UpdateInGroup(typeof(UpdateSystemGroup))]
public class BallMotionSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities
			.WithAllReadOnly<Ball, Velocity>()
			.WithNone<Frozen>()
			.ForEach((ref Position position, ref Velocity velocity) =>
			{
				position.Y += velocity.Y * UnityEngine.Time.deltaTime;
			});
	}
}