using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(EventHandlingSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public class TranslationSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((ref Translation translation, ref Position position) =>
		{
			translation.Value = new float3(GetLinePosition(position.X), position.Y, 0f);
		});
	}

	private float GetLinePosition(Line line)
	{
		switch (line)
		{
			case Line.Left: return -1.5f;
			case Line.Center: return 0f;
			case Line.Right: return 1.5f;
			default: throw new System.InvalidOperationException("unreachable code.");
		}
	}
}