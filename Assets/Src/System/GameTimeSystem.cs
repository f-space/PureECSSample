using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(InputHandlingSystemGroup))]
public class GameTimeSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		if (game.State == GameState.Playing)
		{
			game.ElapsedTime = UnityEngine.Time.deltaTime;
			game.TotalTime += game.ElapsedTime;
		}
		else
		{
			game.ElapsedTime = 0f;
			game.TotalTime = 0f;
		}

		SetSingleton<Game>(game);
	}
}