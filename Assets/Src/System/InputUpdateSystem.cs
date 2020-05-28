using Unity.Entities;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InputSystemGroup), OrderFirst = true)]
[AlwaysUpdateSystem]
public class InputUpdateSystem : SystemBase
{
	protected override void OnCreate()
	{
		InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;
	}

	protected override void OnUpdate()
	{
		InputSystem.Update();
	}
}