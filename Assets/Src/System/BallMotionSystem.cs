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

		this.ForEach((in Game context, ref Position position, ref Velocity velocity) =>
		{
			position.Y += velocity.Y * context.ElapsedTime;
		}, game, this.group);
	}
}