using Unity.Entities;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InputSystemGroup))]
public class MovingPlayerSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		if (game.Phase != GamePhase.GameOver)
		{
			Keyboard keyboard = Keyboard.current;
			if (keyboard != null)
			{
				bool left = keyboard.leftArrowKey.isPressed;
				bool right = keyboard.rightArrowKey.isPressed;
				Line line = left ^ right ? (left ? Line.Left : Line.Right) : Line.Center;

				Entities
					.WithAll<Player, Active>()
					.ForEach((ref Position position) => position.X = line)
					.Run();
			}
		}
	}
}