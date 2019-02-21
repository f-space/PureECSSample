using Unity.Collections;
using Unity.Entities;

[UpdateInGroup(typeof(CollisionGroup))]
[UpdateAfter(typeof(CollisionSystem))]
public class GameOverSystem : ComponentSystem
{
	private ComponentGroup group;

	protected override void OnCreateManager()
	{
		this.group = GetComponentGroup(ComponentType.ReadOnly<Ball>(), ComponentType.ReadOnly<Position>());
	}

	protected override void OnUpdate()
	{
		this.ForEach((in EntityManager manager, ref Position position) =>
		{
			if (position.Y < 0f)
			{
				Entity entity = manager.CreateEntity(System.Array.Empty<ComponentType>());
				manager.AddComponentData(entity, new GameStateChangedEvent { NextState = GameState.GameOver });
			}
		}, EntityManager, this.group);
	}
}