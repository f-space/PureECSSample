using Unity.Entities;

[UpdateInGroup(typeof(EventHandlingSystemGroup))]
public class ResetSystem : ComponentSystem
{
	private EntityQuery eventQuery;
	private EntityQuery ballQuery;

	protected override void OnCreate()
	{
		this.eventQuery = GetEntityQuery(typeof(ResetEvent));
		this.ballQuery = GetEntityQuery(ComponentType.ReadOnly<Ball>());

		RequireForUpdate(this.eventQuery);
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		game.State = GameState.Ready;
		SetSingleton<Game>(game);

		Score score = GetSingleton<Score>();
		score.Value = 0;
		SetSingleton<Score>(score);

		EntityManager.DestroyEntity(this.ballQuery);

		EntityManager.DestroyEntity(this.eventQuery);
	}
}