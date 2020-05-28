using Unity.Entities;

using static Unity.Entities.ComponentType;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
public class ActivationSystem : SystemBase
{
	private EntityQuery activeQuery;
	private EntityQuery inactiveQuery;

	protected override void OnCreate()
	{
		this.activeQuery = GetEntityQuery(new EntityQueryDesc
		{
			All = new[] { ReadOnly<Active>() },
			Any = new[] { ReadOnly<Player>(), ReadOnly<Ball>() },
		});
		this.inactiveQuery = GetEntityQuery(new EntityQueryDesc
		{
			Any = new[] { ReadOnly<Player>(), ReadOnly<Ball>() },
			None = new[] { ReadOnly<Active>() },
		});
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		switch (game.Phase)
		{
			case GamePhase.Ready:
				EntityManager.AddComponent<Active>(this.inactiveQuery);
				break;
			case GamePhase.Playing:
				EntityManager.AddComponent<Active>(this.inactiveQuery);
				break;
			case GamePhase.GameOver:
				EntityManager.RemoveComponent<Active>(this.activeQuery);
				break;
			default:
				throw new System.InvalidOperationException("invalid game state");
		}
	}
}