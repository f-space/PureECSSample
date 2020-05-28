using Unity.Entities;
using Unity.Mathematics;

public struct Game : IComponentData
{
	public GamePhase Phase;
}

public struct Score : IComponentData
{
	public int Value;
}

public enum GamePhase
{
	Ready,
	Playing,
	GameOver,
}

public struct Active : IComponentData { }

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
	public GamePhase Phase;
}

public struct ScoreUI : IComponentData { }

public struct Camera : IComponentData
{
	public float Left;
	public float Right;
	public float Bottom;
	public float Top;
	public float Near;
	public float Far;
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
public struct DynamicText : ISharedComponentData, System.IEquatable<DynamicText>
{
	public UnityEngine.Font Font;

	public bool Equals(DynamicText other) => this.Font == other.Font;
	public override bool Equals(object obj) => (obj is DynamicText other) && this.Equals(other);
	public override int GetHashCode() => this.Font.GetHashCode();
}