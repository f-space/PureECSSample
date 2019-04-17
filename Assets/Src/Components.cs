using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Game : IComponentData
{
	public GameState State;
	public float TotalTime;
	public float ElapsedTime;
	public int Score;
}

public enum GameState
{
	Ready,
	Playing,
	GameOver,
}

public struct GameStateChangedEvent : IComponentData
{
	public GameState NextState;
}

public struct Player : IComponentData { }

public struct Ball : IComponentData { }

public struct Position : IComponentData
{
	public Line X;
	public float Y;
}

public struct Velocity : IComponentData
{
	public float Y;
}

public enum Line
{
	Left = -1,
	Center = 0,
	Right = 1,
}

public struct Frozen : IComponentData { }

public struct Size : IComponentData
{
	public float Height;
}

public struct BallGenerator : IComponentData
{
	public float NextTime;
}

public struct UIPosition : IComponentData
{
	public float X;
	public float Y;
}

[System.Serializable]
public struct Visual : ISharedComponentData
{
	public Mesh Mesh;
	public Material Material;
	public float2 Scale;
}

[System.Serializable]
public struct DynamicText : ISharedComponentData
{
	public Font Font;
}

public struct ReadyUI : IComponentData { }

public struct GameOverUI : IComponentData { }

public struct ScoreUI : IComponentData { }
