using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public static class Boot
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void OnLoad()
	{
		World world = World.Active;
		EntityManager manager = world.GetExistingManager<EntityManager>();
		SystemSwitchingSystem switcher = world.GetExistingManager<SystemSwitchingSystem>();
		Font font = ResourceUtility.CreateAsciiFont(48);

		switcher.Switch(GameState.Ready);

		Entity game = manager.CreateEntity(typeof(Game));
		manager.SetComponentData(game, new Game
		{
			State = GameState.Ready,
			TotalTime = 0f,
			ElapsedTime = 0f,
			Score = 0,
		});

		Entity player = manager.CreateEntity(typeof(Player), typeof(Position), typeof(Size), typeof(Visual));
		manager.SetComponentData(player, new Position { X = Line.Center, Y = 0f });
		manager.SetComponentData(player, new Size { Height = 0.5f });
		manager.SetSharedComponentData(player, new Visual
		{
			Mesh = ResourceUtility.CreateQuad(),
			Material = ResourceUtility.CreateMaterial(Color.white),
			Scale = new float2(1f, 0.25f),
		});

		Entity ball = manager.CreateEntity(typeof(Prefab), typeof(Ball), typeof(Position), typeof(Velocity), typeof(Size), typeof(Visual));
		manager.SetComponentData(ball, new Size { Height = 0.5f });
		manager.SetSharedComponentData(ball, new Visual
		{
			Mesh = ResourceUtility.CreateCircle(),
			Material = ResourceUtility.CreateMaterial(Color.white),
			Scale = new float2(0.5f, 0.5f),
		});

		Entity generator = manager.CreateEntity(typeof(BallGenerator));
		manager.SetComponentData<BallGenerator>(generator, new BallGenerator { NextTime = 0f });

		Entity readyUI = manager.CreateEntity(typeof(Disabled), typeof(UIPosition), typeof(Visual));
		manager.SetComponentData(readyUI, new UIPosition { X = 0f, Y = 4f });
		manager.SetSharedComponentData(readyUI, new Visual
		{
			Mesh = ResourceUtility.CreateTextMesh(font, "Ready?"),
			Material = font.material,
			Scale = new float2(1f, 1f),
		});

		Entity gameoverUI = manager.CreateEntity(typeof(Disabled), typeof(UIPosition), typeof(Visual));
		manager.SetComponentData(gameoverUI, new UIPosition { X = 0f, Y = 4f });
		manager.SetSharedComponentData(gameoverUI, new Visual
		{
			Mesh = ResourceUtility.CreateTextMesh(font, "Game Over"),
			Material = font.material,
			Scale = new float2(1f, 1f),
		});

		Entity scoreUI = manager.CreateEntity(typeof(UIPosition), typeof(Visual), typeof(DynamicTextMesh));
		DynamicTextMeshBuilder scoreBuilder = new DynamicTextMeshBuilder(font);
		manager.SetComponentData(scoreUI, new UIPosition { X = 0f, Y = 3f });
		manager.SetSharedComponentData(scoreUI, new Visual
		{
			Mesh = scoreBuilder.Mesh,
			Material = font.material,
			Scale = new float2(0.5f, 0.5f),
		});
		manager.SetSharedComponentData(scoreUI, new DynamicTextMesh { Builder = scoreBuilder });

		Entity singleton = manager.CreateEntity(typeof(Singleton));
		manager.SetComponentData<Singleton>(singleton, new Singleton
		{
			Player = player,
			BallPrefab = ball,
			ReadyUI = readyUI,
			GameOverUI = gameoverUI,
			ScoreUI = scoreUI,
		});
	}
}