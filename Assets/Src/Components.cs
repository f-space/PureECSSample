using System.Collections.Generic;
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

public struct Visual : ISharedComponentData
{
	public Mesh Mesh;
	public Material Material;
	public float2 Scale;
}

public struct DynamicTextMesh : ISharedComponentData
{
	public DynamicTextMeshBuilder Builder;
}

public struct Singleton : IComponentData
{
	public Entity Player;
	public Entity BallPrefab;
	public Entity ReadyUI;
	public Entity GameOverUI;
	public Entity ScoreUI;
}
