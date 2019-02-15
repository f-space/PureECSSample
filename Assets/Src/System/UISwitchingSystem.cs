using Unity.Entities;

[UpdateInGroup(typeof(UpdateGroup))]
public class UISwitchingSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		Singleton singleton = GetSingleton<Singleton>();

		GameState state = game.State;

		if (EntityManager.HasComponent<Disabled>(singleton.ReadyUI))
		{
			if (state == GameState.Ready)
			{
				EntityManager.RemoveComponent(singleton.ReadyUI, typeof(Disabled));
			}
		}
		else
		{
			if (state != GameState.Ready)
			{
				EntityManager.AddComponent(singleton.ReadyUI, typeof(Disabled));
			}
		}

		if (EntityManager.HasComponent<Disabled>(singleton.GameOverUI))
		{
			if (state == GameState.GameOver)
			{
				EntityManager.RemoveComponent(singleton.GameOverUI, typeof(Disabled));
			}
		}
		else
		{
			if (state != GameState.GameOver)
			{
				EntityManager.AddComponent(singleton.GameOverUI, typeof(Disabled));
			}
		}
	}
}