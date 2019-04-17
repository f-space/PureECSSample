using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(UpdateSystemGroup))]
[UpdateAfter(typeof(BallMotionSystem))]
public class BallGenerationSystem : ComponentSystem
{
	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(ComponentType.ReadOnly<Prefab>(), ComponentType.ReadOnly<Ball>());
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		BallGenerator generator = GetSingleton<BallGenerator>();

		if (generator.NextTime < game.TotalTime)
		{
			Generate(game);

			generator.NextTime += Mathf.Exp(-game.Score / 10f) * 3f + 0.5f;

			SetSingleton<BallGenerator>(generator);
		}
	}

	private void Generate(in Game game)
	{
		Entity prefab = this.query.GetSingletonEntity();

		Entity entity = EntityManager.Instantiate(prefab);
		EntityManager.SetComponentData(entity, new Position { X = SelectLine(), Y = 10f });
		EntityManager.SetComponentData(entity, new Velocity { Y = Mathf.Sqrt(game.Score / 10f + 1) * -3f });
	}

	private Line SelectLine()
	{
		switch (Random.Range(0, 3))
		{
			case 0: return Line.Left;
			case 1: return Line.Center;
			case 2: return Line.Right;
			default: throw new System.InvalidOperationException("unreachable code.");
		}
	}
}