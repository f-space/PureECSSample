using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
[UpdateBefore(typeof(RenderSystem))]
public class ScoreUISystem : SystemBase
{
	private DynamicTextMeshBuilder builder = new DynamicTextMeshBuilder();

	protected override void OnUpdate()
	{
		DynamicTextMeshBuilder builder = this.builder;

		Score score = GetSingleton<Score>();

		Entities
			.WithAll<ScoreUI>()
			.ForEach((in DynamicText dyn, in Visual visual) =>
			{
				builder.Text.Clear();
				builder.Text.Append("SCORE: ");
				builder.Text.Append(score.Value);
				builder.Build(visual.Mesh, dyn.Font);
			})
			.WithoutBurst()
			.Run();
	}
}