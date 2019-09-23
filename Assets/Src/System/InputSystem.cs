using Unity.Entities;

[UpdateBefore(typeof(UpdateSystemGroup))]
public class InputSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		switch (game.State)
		{
			case GameState.Ready:
				if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Return))
				{
					EntityManager.CreateEntity(typeof(GameStartEvent));
				}
				MovePlayer();
				break;
			case GameState.Playing:
				MovePlayer();
				break;
			case GameState.GameOver:
				if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Return))
				{
					EntityManager.CreateEntity(typeof(ResetEvent));
				}
				break;
		}
	}

	private void MovePlayer()
	{
		Entities.WithAllReadOnly<Player>().ForEach((ref Position position) =>
		{
			int sign = 0;
			sign -= UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow) ? 1 : 0;
			sign += UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow) ? 1 : 0;

			position.X =
				sign < 0 ? Line.Left :
				sign > 0 ? Line.Right :
				Line.Center;
		});
	}
}