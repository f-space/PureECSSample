using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public class RenderSystem : ComponentSystem
{
	private EntityQuery query;

	public List<RenderRequest> RenderQueue { get; } = new List<RenderRequest>();

	protected override void OnCreate()
	{
		this.query = GetEntityQuery(new EntityQueryDesc()
		{
			All = new[] { ComponentType.ReadOnly<Visual>() },
			Any = new[] { ComponentType.ReadOnly<Position>(), ComponentType.ReadOnly<UIPosition>() }
		});
	}

	protected override void OnUpdate()
	{
		ArchetypeChunkSharedComponentType<Visual> visualType = GetArchetypeChunkSharedComponentType<Visual>();
		ArchetypeChunkComponentType<Position> positionType = GetArchetypeChunkComponentType<Position>(true);
		ArchetypeChunkComponentType<UIPosition> uiPositionType = GetArchetypeChunkComponentType<UIPosition>(true);

		RenderQueue.Clear();

		using (NativeArray<ArchetypeChunk> chunks = this.query.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				Visual visual = chunk.GetSharedComponentData<Visual>(visualType, EntityManager);
				if (chunk.Has(positionType))
				{
					NativeArray<Position> positions = chunk.GetNativeArray(positionType);
					foreach (Position position in positions)
					{
						Request(visual, GetLinePosition(position.X), position.Y);
					}
				}
				else if (chunk.Has(uiPositionType))
				{
					NativeArray<UIPosition> positions = chunk.GetNativeArray(uiPositionType);
					foreach (UIPosition position in positions)
					{
						Request(visual, position.X, position.Y);
					}
				}
			}
		}
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

	private void Request(in Visual visual, float x, float y)
	{
		RenderQueue.Add(new RenderRequest
		{
			Mesh = visual.Mesh,
			Material = visual.Material,
			Scale = visual.Scale,
			Position = new UnityEngine.Vector2(x, y),
		});
	}
}