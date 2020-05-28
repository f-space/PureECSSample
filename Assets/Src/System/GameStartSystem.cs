using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InputSystemGroup))]
public class GameStartSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		if (game.Phase == GamePhase.Ready)
		{
			Keyboard keyboard = Keyboard.current;
			if (keyboard != null && keyboard.enterKey.wasPressedThisFrame)
			{
				CreateBallGenerator();

				SetSingleton(new Game { Phase = GamePhase.Playing });
			}
		}
	}

	private void CreateBallGenerator()
	{
		float time = (float)Time.ElapsedTime;
		uint seed = System.BitConverter.ToUInt32(System.BitConverter.GetBytes(time), 0);

		Entity entity = EntityManager.CreateEntity(typeof(BallGenerator));
		SetComponent(entity, new BallGenerator
		{
			Random = new Random(seed),
			NextTime = time,
		});
	}
}