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

		this.ForEach((in Game context, ref Position position, ref Velocity velocity) =>
		{
			position.Y += velocity.Y * context.ElapsedTime;
		}, game, this.query);
	}
}