using Unity.Entities;
using Unity.Mathematics;

public struct Game : IComponentData
{
	public GameState State;
}

public enum GameState
{
	Ready,
	Playing,
	GameOver,
}

public struct GameStartEvent : IComponentData { }

public struct GameOverEvent : IComponentData { }

public struct ResetEvent : IComponentData { }

public struct Player : IComponentData { }

public struct Ball : IComponentData { }

public enum Line
{
	Left,
	Center,
	Right,
}

public struct Position : IComponentData
{
	public Line X;
	public float Y;
}

public struct Velocity : IComponentData
{
	public float Y;
}

public struct Frozen : IComponentData { }

public struct HitBoxSize : IComponentData
{
	public float Height;
}

public struct BallGenerator : IComponentData
{
	public Random Random;
	public float NextTime;
}

public struct VisibleWhile : IComponentData
{
	public GameState State;
}

public struct Score : IComponentData
{
	public int Value;
}

[System.Serializable]
public struct Visual : ISharedComponentData, System.IEquatable<Visual>
{
	public UnityEngine.Mesh Mesh;
	public UnityEngine.Material Material;

	public bool Equals(Visual other) => this.Mesh == other.Mesh && this.Material == other.Material;
	public override bool Equals(object obj) => (obj is Visual other) && this.Equals(other);
	public override int GetHashCode() => this.Mesh.GetHashCode() ^ this.Material.GetHashCode();
}

[System.Serializable]
public struct WithFont : ISharedComponentData, System.IEquatable<WithFont>
{
	public UnityEngine.Font Font;

	public bool Equals(WithFont other) => this.Font == other.Font;
	public override bool Equals(object obj) => (obj is WithFont other) && this.Equals(other);
	public override int GetHashCode() => this.Font.GetHashCode();
}