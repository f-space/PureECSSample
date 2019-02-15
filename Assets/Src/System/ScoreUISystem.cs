using Unity.Entities;

[UpdateBefore(typeof(RenderGroup))]
public class ScoreUISystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Game game = GetSingleton<Game>();
		Entity scoreUI = GetSingleton<Singleton>().ScoreUI;
		DynamicTextMesh mesh = EntityManager.GetSharedComponentData<DynamicTextMesh>(scoreUI);

		DynamicTextMeshBuilder builder = mesh.Builder;
		int score = game.Score;

		builder.Text.Clear();
		builder.Text.Append("SCORE: ");
		builder.Text.Append(score);
		builder.Build();
	}
}