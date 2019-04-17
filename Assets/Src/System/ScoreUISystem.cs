using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(RenderSystem))]
public class ScoreUISystem : ComponentSystem
{
	private DynamicTextMeshBuilder builder;

	protected override void OnCreate()
	{
		this.builder = new DynamicTextMeshBuilder();
	}

	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		Entity scoreUI = GetSingletonEntity<ScoreUI>();
		Visual visual = EntityManager.GetSharedComponentData<Visual>(scoreUI);
		DynamicText text = EntityManager.GetSharedComponentData<DynamicText>(scoreUI);

		this.builder.Text.Clear();
		this.builder.Text.Append("SCORE: ");
		this.builder.Text.Append(game.Score);
		this.builder.Build(visual.Mesh, text.Font);
	}
}