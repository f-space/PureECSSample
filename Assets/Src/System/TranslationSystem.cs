using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(InteractionSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public class TranslationSystem : SystemBase
{
	protected override void OnUpdate()
	{
		Entities
			.WithChangeFilter<Position>()
			.ForEach((ref Translation translation, in Position position) =>
			{
				translation.Value = new float3(GetLinePosition(position.X), position.Y, 0f);
			})
			.Schedule();
	}

	private static float GetLinePosition(Line line)
	{
		switch (line)
		{
			case Line.Left: return -1.5f;
			case Line.Center: return 0f;
			case Line.Right: return 1.5f;
			default: throw new System.InvalidOperationException("unreachable code");
		}
	}
}