using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(UpdateSystemGroup), OrderLast = true)]
public class GameOverSystem : SystemBase
{
	private EntityQuery ballQuery;

	private EntityCommandBufferSystem ecbSystem;

	protected override void OnCreate()
	{
		this.ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();

		RequireForUpdate(this.ballQuery);
	}

	protected override void OnUpdate()
	{
		Entity gameEntity = GetSingletonEntity<Game>();
		Entity ganeratorEntity = GetSingletonEntity<BallGenerator>();

		EntityCommandBuffer ecb = this.ecbSystem.CreateCommandBuffer();

		NativeArray<bool> gameOver = new NativeArray<bool>(1, Allocator.TempJob);

		Entities
			.WithAll<Ball, Active>()
			.ForEach((in Position position) => { if (position.Y < 0f) gameOver[0] = true; })
			.WithStoreEntityQueryInField(ref this.ballQuery)
			.Schedule();

		Job
			.WithCode(() =>
			{
				if (gameOver[0])
				{
					ecb.DestroyEntity(ganeratorEntity);
					SetComponent(gameEntity, new Game { Phase = GamePhase.GameOver });
				}
			})
			.WithDeallocateOnJobCompletion(gameOver)
			.Schedule();

		this.ecbSystem.AddJobHandleForProducer(this.Dependency);
	}
}