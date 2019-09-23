using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class UpdateSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(UpdateSystemGroup))]
public class InteractionSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(InteractionSystemGroup))]
public class EventHandlingSystemGroup : ComponentSystemGroup { }