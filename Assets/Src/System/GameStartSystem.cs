using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(EventHandlingSystemGroup))]
public class GameStartSystem : ComponentSystem
{
	private EntityQuery eventQuery;

	protected override void OnCreate()
	{
		this.eventQuery = GetEntityQuery(typeof(GameStartEvent));

		RequireForUpdate(this.eventQuery);
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		game.State = GameState.Playing;
		SetSingleton<Game>(game);

		Entity generator = EntityManager.CreateEntity(typeof(BallGenerator));
		EntityManager.SetComponentData(generator, new BallGenerator
		{
			Random = new Random((uint)UnityEngine.Time.frameCount),
			NextTime = UnityEngine.Time.time,
		});

		EntityManager.DestroyEntity(this.eventQuery);
	}
}