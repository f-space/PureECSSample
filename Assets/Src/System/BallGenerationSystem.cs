using Unity.Entities;
using Unity.Mathematics;

using static Unity.Mathematics.math;

[UpdateInGroup(typeof(UpdateSystemGroup))]
[UpdateAfter(typeof(BallMotionSystem))]
public class BallGenerationSystem : ComponentSystem
{
	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = Entities.WithAllReadOnly<Prefab, Ball>().ToEntityQuery();

		RequireSingletonForUpdate<BallGenerator>();
	}

	protected override void OnUpdate()
	{
		BallGenerator generator = GetSingleton<BallGenerator>();

		if (generator.NextTime < UnityEngine.Time.time)
		{
			Score score = GetSingleton<Score>();
			Position position = new Position { X = RandomLine(ref generator.Random), Y = 10f };
			Velocity velocity = new Velocity { Y = sqrt(score.Value / 10f + 1) * -3f };

			Generate(position, velocity);

			generator.NextTime += exp(-score.Value / 10f) * 3f + 0.5f;

			SetSingleton<BallGenerator>(generator);
		}
	}

	private void Generate(Position position, Velocity velocity)
	{
		Entity prefab = this.query.GetSingletonEntity();

		Entity entity = EntityManager.Instantiate(prefab);
		EntityManager.SetComponentData(entity, position);
		EntityManager.SetComponentData(entity, velocity);
	}

	private Line RandomLine(ref Random random)
	{
		switch (random.NextInt(3))
		{
			case 0: return Line.Left;
			case 1: return Line.Center;
			case 2: return Line.Right;
			default: throw new System.InvalidOperationException("unreachable code.");
		}
	}
}