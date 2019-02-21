using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(CollisionGroup))]
public class CollisionSystem : ComponentSystem
{
	private ComponentGroup group;

	protected override void OnCreateManager()
	{
		this.group = GetComponentGroup(ComponentType.ReadOnly<Ball>(), ComponentType.ReadOnly<Position>(), ComponentType.ReadOnly<Size>());
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		Entity player = GetSingleton<Singleton>().Player;
		Position playerPosition = EntityManager.GetComponentData<Position>(player);
		Size playerSize = EntityManager.GetComponentData<Size>(player);

		(ComponentSystem, Game game, Position, Size) context = (this, game, playerPosition, playerSize);

		this.ForEach((ref (ComponentSystem @this, Game game, Position, Size) ctx, Entity entity, ref Position position, ref Size size) =>
		{
			(_, _, Position pPos, Size pSize) = ctx;
			if (position.X == pPos.X && position.Y - size.Height / 2f < pPos.Y + pSize.Height / 2f)
			{
				ctx.game.Score += 1;
				ctx.@this.PostUpdateCommands.DestroyEntity(entity);
			}
		}, ref context, this.group);

		SetSingleton<Game>(context.game);
	}
}