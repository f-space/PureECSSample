using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(InteractionSystemGroup))]
public class CollisionSystem : SystemBase
{
	private struct PlayerHitBox
	{
		public Line X;
		public float Top;
	}

	private EntityQuery playerQuery;

	private EntityQuery ballQuery;

	private EntityCommandBufferSystem ecbSystem;

	protected override void OnCreate()
	{
		this.ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();

		RequireForUpdate(playerQuery);
		RequireForUpdate(ballQuery);
	}

	protected override void OnUpdate()
	{
		EntityCommandBuffer ecb = this.ecbSystem.CreateCommandBuffer();

		int playerCount = this.playerQuery.CalculateEntityCount();
		NativeArray<PlayerHitBox> players = new NativeArray<PlayerHitBox>(playerCount, Allocator.TempJob);
		NativeArray<int> count = new NativeArray<int>(1, Allocator.TempJob);

		Entities
			.WithAll<Player, Active>()
			.ForEach((int entityInQueryIndex, in Position position, in HitBoxSize size) =>
			{
				players[entityInQueryIndex] = new PlayerHitBox
				{
					X = position.X,
					Top = position.Y + size.Height / 2f,
				};
			})
			.WithStoreEntityQueryInField(ref this.playerQuery)
			.Schedule();

		Entities
			.WithAll<Ball, Active>()
			.ForEach((Entity entity, in Position position, in HitBoxSize size) =>
			{
				Line ballX = position.X;
				float ballBottom = position.Y - size.Height / 2f;
				for (int i = 0; i < players.Length; i++)
				{
					PlayerHitBox player = players[i];
					if (ballX == player.X && ballBottom < player.Top)
					{
						count[0]++;
						ecb.DestroyEntity(entity);
						break;
					}
				}
			})
			.WithStoreEntityQueryInField(ref this.ballQuery)
			.WithReadOnly(players)
			.WithDeallocateOnJobCompletion(players)
			.Schedule();

		Entities
			.ForEach((ref Score score) => score.Value += count[0])
			.WithReadOnly(count)
			.WithDeallocateOnJobCompletion(count)
			.Schedule();

		this.ecbSystem.AddJobHandleForProducer(this.Dependency);
	}
}