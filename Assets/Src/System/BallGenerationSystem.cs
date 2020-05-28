using Unity.Entities;
using Unity.Mathematics;

using static Unity.Entities.ComponentType;
using static Unity.Mathematics.math;

[UpdateInGroup(typeof(UpdateSystemGroup))]
public class BallGenerationSystem : SystemBase
{
	private struct GenResult
	{
		public Position Position;
		public Velocity Velocity;
		public float NextInterval;
	}

	private EntityQuery ballPrefabQuery;

	private EntityCommandBufferSystem ecbSystem;

	protected override void OnCreate()
	{
		this.ballPrefabQuery = GetEntityQuery(ReadOnly<Prefab>(), ReadOnly<Ball>());
		this.ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();

		RequireSingletonForUpdate<BallGenerator>();
	}

	protected override void OnUpdate()
	{
		Entity prefab = this.ballPrefabQuery.GetSingletonEntity();
		Score score = GetSingleton<Score>();
		float time = (float)Time.ElapsedTime;

		EntityCommandBuffer ecb = this.ecbSystem.CreateCommandBuffer();

		Entities.ForEach((ref BallGenerator gen) =>
		{
			if (gen.NextTime < time)
			{
				GenResult result = Generate(ref gen.Random, score.Value);

				Entity entity = ecb.Instantiate(prefab);
				ecb.SetComponent(entity, result.Position);
				ecb.SetComponent(entity, result.Velocity);

				gen.NextTime += result.NextInterval;
			}
		}).Schedule();

		this.ecbSystem.AddJobHandleForProducer(this.Dependency);
	}

	private static GenResult Generate(ref Random random, int score)
	{
		Line x = RandomLine(ref random);
		float y = 10f;
		float v = sqrt(score / 10f + 1f) * -3f;
		float t = exp(-score / 10f) * 3f + 0.5f;

		return new GenResult
		{
			Position = new Position { X = x, Y = y },
			Velocity = new Velocity { Y = v },
			NextInterval = t,
		};
	}

	private static Line RandomLine(ref Random random)
	{
		switch (random.NextInt(3))
		{
			case 0: return Line.Left;
			case 1: return Line.Center;
			case 2: return Line.Right;
			default: throw new System.InvalidOperationException("unreachable code");
		}
	}
}