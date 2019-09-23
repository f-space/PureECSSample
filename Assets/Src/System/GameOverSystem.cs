using Unity.Entities;

[UpdateInGroup(typeof(EventHandlingSystemGroup))]
public class GameOverSystem : ComponentSystem
{
	private EntityQuery eventQuery;

	private EntityQuery ballQuery;

	protected override void OnCreate()
	{
		this.eventQuery = GetEntityQuery(typeof(GameOverEvent));
		this.ballQuery = GetEntityQuery(ComponentType.ReadOnly<Ball>());

		RequireForUpdate(this.eventQuery);
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		game.State = GameState.GameOver;
		SetSingleton<Game>(game);

		EntityManager.DestroyEntity(GetSingletonEntity<BallGenerator>());
		
		EntityManager.AddComponent(this.ballQuery, typeof(Frozen));

		EntityManager.DestroyEntity(this.eventQuery);
	}
}