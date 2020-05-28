using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class InputSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(InputSystemGroup))]
public class UpdateSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(UpdateSystemGroup))]
public class InteractionSystemGroup : ComponentSystemGroup { }
