using Unity.Entities;

public class UpdateGroup { }

[UpdateAfter(typeof(UpdateGroup))]
public class CollisionGroup { }

[UpdateAfter(typeof(CollisionGroup))]
public class EventHandlingGroup { }

[UpdateAfter(typeof(EventHandlingGroup))]
public class RenderGroup { }
