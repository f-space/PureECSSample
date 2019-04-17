using Unity.Entities;

[UpdateInGroup(typeof(InteractionSystemGroup))]
public class CollisionSystem : ComponentSystem
{
	private struct Context
	{
		public ComponentSystem System;
		public Game Game;
		public Position PlayerPosition;
		public Size PlayerSize;
		public bool GameOver;
	}

	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(new EntityQueryDesc
		{
			All = new[] { ComponentType.ReadOnly<Ball>(), ComponentType.ReadOnly<Position>(), ComponentType.ReadOnly<Size>() },
			None = new[] { ComponentType.ReadWrite<Frozen>() },
		});
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		Entity player = GetSingletonEntity<Player>();
		Position playerPosition = EntityManager.GetComponentData<Position>(player);
		Size playerSize = EntityManager.GetComponentData<Size>(player);

		Context context = new Context
		{
			System = this,
			Game = game,
			PlayerPosition = playerPosition,
			PlayerSize = playerSize,
			GameOver = false,
		};

		this.ForEach((ref Context ctx, Entity entity, ref Position position, ref Size size) =>
		{
			Position pPos = ctx.PlayerPosition;
			Size pSize = ctx.PlayerSize;
			if (position.X == pPos.X && position.Y - size.Height / 2f < pPos.Y + pSize.Height / 2f)
			{
				ctx.Game.Score += 1;
				ctx.System.PostUpdateCommands.DestroyEntity(entity);
			}
			else if (position.Y < 0f)
			{
				ctx.GameOver = true;
			}
		}, ref context, this.query);

		if (context.GameOver)
		{
			Entity entity = EntityManager.CreateEntity();
			EntityManager.AddComponentData(entity, new GameStateChangedEvent { NextState = GameState.GameOver });
		}

		SetSingleton<Game>(context.Game);
	}
}