using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public static class Boot
{
	public static ComponentType[] GameArchetype = new ComponentType[] {
		typeof(Game),
		typeof(Score),
	};

	public static ComponentType[] CameraArchetype = new ComponentType[] {
		typeof(LocalToWorld),
		typeof(Translation),
		typeof(NonUniformScale),
		typeof(Camera),
	};

	public static ComponentType[] PlayerArchetype = new ComponentType[] {
		typeof(LocalToWorld),
		typeof(Translation),
		typeof(NonUniformScale),
		typeof(Player),
		typeof(Position),
		typeof(HitBoxSize),
		typeof(Visual),
	};

	public static ComponentType[] EnemyPrefabArchetype = new ComponentType[] {
		typeof(Prefab),
		typeof(LocalToWorld),
		typeof(Translation),
		typeof(Scale),
		typeof(Ball),
		typeof(Position),
		typeof(Velocity),
		typeof(HitBoxSize),
		typeof(Visual),
	};

	public static ComponentType[] ReadyUIArchetype = new ComponentType[] {
		typeof(LocalToWorld),
		typeof(Translation),
		typeof(VisibleWhile),
		typeof(Visual),
	};

	public static ComponentType[] GameoverUIArchetype = new ComponentType[] {
		typeof(LocalToWorld),
		typeof(Translation),
		typeof(VisibleWhile),
		typeof(Visual),
	};

	public static ComponentType[] ScoreUIArchetype = new ComponentType[] {
		typeof(LocalToWorld),
		typeof(Translation),
		typeof(Scale),
		typeof(ScoreUI),
		typeof(DynamicText),
		typeof(Visual),
	};

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void OnLoad()
	{
		World world = World.DefaultGameObjectInjectionWorld;
		EntityManager manager = world.EntityManager;

		EnableFixedRateSimulation(world);

		Font font = ResourceUtility.CreateAsciiFont(48);
		CreateGameEntity(manager);
		CreateCameraEntity(manager);
		CreatePlayerEntity(manager);
		CreateEnemyPrefabEntity(manager);
		CreateReadyUIEntity(manager, font);
		CreateGameoverUIEntity(manager, font);
		CreateScoreUIEntity(manager, font);
	}

	private static void EnableFixedRateSimulation(World world)
	{
		SimulationSystemGroup group = world.GetExistingSystem<SimulationSystemGroup>();
		FixedRateUtils.EnableFixedRateWithCatchUp(group, Time.fixedDeltaTime);
	}

	private static void CreateGameEntity(EntityManager manager)
	{
		Entity entity = manager.CreateEntity(GameArchetype);
		manager.SetComponentData(entity, new Game { Phase = GamePhase.Ready });
		manager.SetComponentData(entity, new Score { Value = 0 });
	}

	private static void CreateCameraEntity(EntityManager manager)
	{
		Entity entity = manager.CreateEntity(CameraArchetype);
		manager.SetComponentData(entity, new Translation() { Value = new float3(0f, 0f, -1f) });
		manager.SetComponentData(entity, new NonUniformScale() { Value = new float3(1f, 1f, -1f) });
		manager.SetComponentData(entity, new Camera
		{
			Left = -2.5f,
			Right = 2.5f,
			Bottom = -1f,
			Top = 7f,
			Near = 0f,
			Far = 2f,
		});
	}

	private static void CreatePlayerEntity(EntityManager manager)
	{
		Entity entity = manager.CreateEntity(PlayerArchetype);
		manager.SetComponentData(entity, new NonUniformScale { Value = new float3(1f, 0.25f, 1f) });
		manager.SetComponentData(entity, new Position { X = Line.Center, Y = 0f });
		manager.SetComponentData(entity, new HitBoxSize { Height = 0.5f });
		manager.SetSharedComponentData(entity, new Visual
		{
			Mesh = ResourceUtility.CreateQuad(),
			Material = ResourceUtility.CreateMaterial(Color.white),
		});
	}

	private static void CreateEnemyPrefabEntity(EntityManager manager)
	{
		Entity entity = manager.CreateEntity(EnemyPrefabArchetype);
		manager.SetComponentData(entity, new Scale { Value = 0.5f });
		manager.SetComponentData(entity, new HitBoxSize { Height = 0.5f });
		manager.SetSharedComponentData(entity, new Visual
		{
			Mesh = ResourceUtility.CreateCircle(),
			Material = ResourceUtility.CreateMaterial(Color.white),
		});
	}

	private static void CreateReadyUIEntity(EntityManager manager, Font font)
	{
		Entity entity = manager.CreateEntity(ReadyUIArchetype);
		manager.SetComponentData(entity, new Translation { Value = new float3(0f, 4f, 0f) });
		manager.SetComponentData(entity, new VisibleWhile { Phase = GamePhase.Ready });
		manager.SetSharedComponentData(entity, new Visual
		{
			Mesh = ResourceUtility.CreateTextMesh(font, "Ready?"),
			Material = font.material,
		});
	}

	private static void CreateGameoverUIEntity(EntityManager manager, Font font)
	{
		Entity entity = manager.CreateEntity(GameoverUIArchetype);
		manager.SetComponentData(entity, new Translation { Value = new float3(0f, 4f, 0f) });
		manager.SetComponentData(entity, new VisibleWhile { Phase = GamePhase.GameOver });
		manager.SetSharedComponentData(entity, new Visual
		{
			Mesh = ResourceUtility.CreateTextMesh(font, "Game Over"),
			Material = font.material,
		});
	}

	private static void CreateScoreUIEntity(EntityManager manager, Font font)
	{
		Entity entity = manager.CreateEntity(ScoreUIArchetype);
		manager.SetComponentData(entity, new Translation { Value = new float3(0f, 3f, 0f) });
		manager.SetComponentData(entity, new Scale { Value = 0.5f });
		manager.SetSharedComponentData(entity, new DynamicText { Font = font });
		manager.SetSharedComponentData(entity, new Visual
		{
			Mesh = ResourceUtility.CreateDynamicMesh(),
			Material = font.material,
		});
	}
}