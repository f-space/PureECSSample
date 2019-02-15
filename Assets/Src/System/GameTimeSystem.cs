using Unity.Entities;

[UpdateBefore(typeof(UpdateGroup))]
public class GameTimeSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		game.ElapsedTime = UnityEngine.Time.deltaTime;
		game.TotalTime += game.ElapsedTime;

		SetSingleton<Game>(game);
	}
}