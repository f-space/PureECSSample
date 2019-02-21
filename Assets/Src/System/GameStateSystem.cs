using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(EventHandlingGroup))]
public class GameStateSystem : ComponentSystem
{
	private ComponentGroup group;

	protected override void OnCreateManager()
	{
		this.group = GetComponentGroup(ComponentType.Create<GameStateChangedEvent>());
	}

	protected override void OnUpdate()
	{
		this.ForEach((in ComponentSystem @this, Entity entity, ref GameStateChangedEvent ev) =>
		{
			Game game = @this.GetSingleton<Game>();
			game.State = ev.NextState;
			@this.SetSingleton<Game>(game);

			@this.PostUpdateCommands.DestroyEntity(entity);
		}, this, this.group);
	}
}