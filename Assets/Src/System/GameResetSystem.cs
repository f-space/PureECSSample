using Unity.Entities;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InputSystemGroup))]
[UpdateAfter(typeof(GameStartSystem))]
public class GameResetSystem : SystemBase
{
	private EntityQuery query;

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(typeof(Ball));
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		if (game.Phase == GamePhase.GameOver)
		{
			Keyboard keyboard = Keyboard.current;
			if (keyboard != null && keyboard.enterKey.wasPressedThisFrame)
			{
				EntityManager.DestroyEntity(this.query);

				SetSingleton<Game>(new Game { Phase = GamePhase.Ready });
				SetSingleton<Score>(new Score { Value = 0 });
			}
		}
	}
}