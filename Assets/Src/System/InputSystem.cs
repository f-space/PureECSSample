using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InputHandlingSystemGroup))]
public class InputSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		switch (game.State)
		{
			case GameState.Ready:
				if (Input.GetKeyDown(KeyCode.Return)) ChangeState(GameState.Playing);
				MovePlayer();
				break;
			case GameState.Playing:
				MovePlayer();
				break;
			case GameState.GameOver:
				if (Input.GetKeyDown(KeyCode.Return)) ChangeState(GameState.Ready);
				break;
		}
	}

	private void ChangeState(GameState state)
	{
		Entity entity = EntityManager.CreateEntity();
		EntityManager.AddComponentData(entity, new GameStateChangedEvent { NextState = state });
	}

	private void MovePlayer()
	{
		Entity player = GetSingletonEntity<Player>();
		Position position = EntityManager.GetComponentData<Position>(player);

		int sign = 0;
		sign -= Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
		sign += Input.GetKey(KeyCode.RightArrow) ? 1 : 0;

		position.X =
			sign < 0 ? Line.Left :
			sign > 0 ? Line.Right :
			Line.Center;

		EntityManager.SetComponentData<Position>(player, position);
	}
}