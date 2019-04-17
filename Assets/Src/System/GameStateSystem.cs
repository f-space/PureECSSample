using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameStateSystem : ComponentSystem
{
	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(typeof(GameStateChangedEvent));
	}

	protected override void OnUpdate()
	{
		this.ForEach((in ComponentSystem @this, Entity entity, ref GameStateChangedEvent ev) =>
		{
			Game game = @this.GetSingleton<Game>();
			game.State = ev.NextState;
			@this.SetSingleton<Game>(game);

			@this.PostUpdateCommands.DestroyEntity(entity);
		}, this, this.query);
	}
}