using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public static class Boot
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void OnLoad()
	{
		World world = World.Active;
		EntityManager manager = world.EntityManager;
		Font font = ResourceUtility.CreateAsciiFont(48);

		Entity game = manager.CreateEntity(typeof(Game));
		manager.SetComponentData(game, new Game
		{
			State = GameState.Ready,
		});

		Entity player = manager.CreateEntity(
			typeof(Player),
			typeof(Position),
			typeof(HitBoxSize),
			typeof(Visual),
			typeof(LocalToWorld),
			typeof(Translation),
			typeof(NonUniformScale)
		);
		manager.SetComponentData(player, new Position { X = Line.Center, Y = 0f });
		manager.SetComponentData(player, new HitBoxSize { Height = 0.5f });
		manager.SetSharedComponentData(player, new Visual
		{
			Mesh = ResourceUtility.CreateQuad(),
			Material = ResourceUtility.CreateMaterial(Color.white),
		});
		manager.SetComponentData(player, new NonUniformScale { Value = new float3(1f, 0.25f, 1f) });

		Entity ball = manager.CreateEntity(
			typeof(Prefab),
			typeof(Ball),
			typeof(Position),
			typeof(Velocity),
			typeof(HitBoxSize),
			typeof(Visual),
			typeof(LocalToWorld),
			typeof(Translation),
			typeof(Scale)
		);
		manager.SetComponentData(ball, new HitBoxSize { Height = 0.5f });
		manager.SetSharedComponentData(ball, new Visual
		{
			Mesh = ResourceUtility.CreateCircle(),
			Material = ResourceUtility.CreateMaterial(Color.white),
		});
		manager.SetComponentData(ball, new Scale { Value = 0.5f });

		Entity readyUI = manager.CreateEntity(
			typeof(Visual),
			typeof(VisibleWhile),
			typeof(LocalToWorld),
			typeof(Translation)
		);
		manager.SetSharedComponentData(readyUI, new Visual
		{
			Mesh = ResourceUtility.CreateTextMesh(font, "Ready?"),
			Material = font.material,
		});
		manager.SetComponentData(readyUI, new VisibleWhile { State = GameState.Ready });
		manager.SetComponentData(readyUI, new Translation { Value = new float3(0f, 4f, 0f) });

		Entity gameoverUI = manager.CreateEntity(
			typeof(Visual),
			typeof(VisibleWhile),
			typeof(LocalToWorld),
			typeof(Translation)
		);
		manager.SetSharedComponentData(gameoverUI, new Visual
		{
			Mesh = ResourceUtility.CreateTextMesh(font, "Game Over"),
			Material = font.material,
		});
		manager.SetComponentData(gameoverUI, new VisibleWhile { State = GameState.GameOver });
		manager.SetComponentData(gameoverUI, new Translation { Value = new float3(0f, 4f, 0f) });

		Entity scoreUI = manager.CreateEntity(
			typeof(Score),
			typeof(Visual),
			typeof(WithFont),
			typeof(LocalToWorld),
			typeof(Translation),
			typeof(Scale)
		);
		manager.SetComponentData(scoreUI, new Score { Value = 0 });
		manager.SetSharedComponentData(scoreUI, new Visual
		{
			Mesh = ResourceUtility.CreateDynamicMesh(),
			Material = font.material,
		});
		manager.SetSharedComponentData(scoreUI, new WithFont { Font = font });
		manager.SetComponentData(scoreUI, new Translation { Value = new float3(0f, 3f, 0f) });
		manager.SetComponentData(scoreUI, new Scale { Value = 0.5f });
	}
}