using Unity.Entities;

[UpdateInGroup(typeof(InteractionSystemGroup))]
public class CollisionSystem : ComponentSystem
{
	private EntityQuery ballQuery;

	protected override void OnCreate()
	{
		this.ballQuery = Entities
			.WithAllReadOnly<Ball, Position, HitBoxSize>()
			.WithNone<Frozen>()
			.ToEntityQuery();

		RequireForUpdate(this.ballQuery);
	}

	protected override void OnUpdate()
	{
		Score score = GetSingleton<Score>();
		Entity player = GetSingletonEntity<Player>();
		Position playerPosition = EntityManager.GetComponentData<Position>(player);
		HitBoxSize playerSize = EntityManager.GetComponentData<HitBoxSize>(player);
		bool gameOver = false;

		Entities.With(this.ballQuery).ForEach((Entity entity, ref Position position, ref HitBoxSize size) =>
		{
			if (position.X == playerPosition.X && position.Y - size.Height / 2f < playerPosition.Y + playerSize.Height / 2f)
			{
				score.Value += 1;
				EntityManager.DestroyEntity(entity);
			}
			else if (position.Y < 0f)
			{
				gameOver = true;
			}
		});

		SetSingleton<Score>(score);

		if (gameOver) EntityManager.CreateEntity(typeof(GameOverEvent));
	}
}