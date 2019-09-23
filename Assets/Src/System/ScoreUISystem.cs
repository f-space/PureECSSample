using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(RenderSystem))]
public class ScoreUISystem : ComponentSystem
{
	private EntityQuery query;

	private DynamicTextMeshBuilder builder;

	protected override void OnCreate()
	{
		this.query = Entities.WithAllReadOnly<Score, WithFont>().WithAll<Visual>().ToEntityQuery();

		this.builder = new DynamicTextMeshBuilder();
	}

	protected override void OnUpdate()
	{
		Entity entity = query.GetSingletonEntity();
		Score score = EntityManager.GetComponentData<Score>(entity);
		Visual visual = EntityManager.GetSharedComponentData<Visual>(entity);
		WithFont font = EntityManager.GetSharedComponentData<WithFont>(entity);

		this.builder.Text.Clear();
		this.builder.Text.Append("SCORE: ");
		this.builder.Text.Append(score.Value);
		this.builder.Build(visual.Mesh, font.Font);
	}
}