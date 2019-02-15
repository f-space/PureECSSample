using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(GameStateSystem))]
public class SystemSwitchingSystem : ComponentSystem
{
	private static (System.Type, int)[] Systems = {
		(typeof(GameTimeSystem), ToFlag(GameState.Playing)),
		(typeof(BallGenerationSystem), ToFlag(GameState.Playing)),
		(typeof(BallMotionSystem), ToFlag(GameState.Playing)),
		(typeof(CollisionSystem), ToFlag(GameState.Playing)),
		(typeof(GameOverSystem), ToFlag(GameState.Playing)),
	};

	public void Switch(GameState state)
	{
		int bit = ToFlag(state);
		foreach ((System.Type type, int flags) in Systems)
		{
			if (World.GetExistingManager(type) is ComponentSystem system)
			{
				system.Enabled = ((flags & bit) != 0);
			}
		}
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();

		Switch(game.State);
	}

	private static int ToFlag(GameState state) => 1 << (int)state;
}