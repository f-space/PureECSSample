using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

public static class ComponentSystemExtensions
{
	public delegate void F_RD<R, T0>(in R context, ref T0 c0)
		where T0 : struct, IComponentData;

	public static unsafe void ForEach<R, T0>(this ComponentSystem @this, F_RD<R, T0> operate, in R context, ComponentGroup group)
		where T0 : struct, IComponentData
	{
		ArchetypeChunkComponentType<T0> chunkComponentType0 = @this.GetArchetypeChunkComponentType<T0>(false);

		using (NativeArray<ArchetypeChunk> chunks = group.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				int length = chunk.Count;
				void* array0 = chunk.GetNativeArray(chunkComponentType0).GetUnsafePtr();
				for (int i = 0; i < length; i++)
				{
					ref T0 c0 = ref UnsafeUtilityEx.ArrayElementAsRef<T0>(array0, i);
					operate(context, ref c0);
				}
			}
		}
	}

	public delegate void F_RED<R, T0>(in R context, Entity entity, ref T0 c0)
		where T0 : struct, IComponentData;

	public static unsafe void ForEach<R, T0>(this ComponentSystem @this, F_RED<R, T0> operate, in R context, ComponentGroup group)
		where T0 : struct, IComponentData
	{
		ArchetypeChunkEntityType entityType = @this.GetArchetypeChunkEntityType();
		ArchetypeChunkComponentType<T0> chunkComponentType0 = @this.GetArchetypeChunkComponentType<T0>(false);

		using (NativeArray<ArchetypeChunk> chunks = group.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				int length = chunk.Count;
				NativeArray<Entity> entities = chunk.GetNativeArray(entityType);
				void* array0 = chunk.GetNativeArray(chunkComponentType0).GetUnsafePtr();
				for (int i = 0; i < length; i++)
				{
					ref T0 c0 = ref UnsafeUtilityEx.ArrayElementAsRef<T0>(array0, i);
					operate(context, entities[i], ref c0);
				}
			}
		}
	}

	public delegate void F_RDD<R, T0, T1>(in R context, ref T0 c0, ref T1 c1)
		where T0 : struct, IComponentData
		where T1 : struct, IComponentData;

	public static unsafe void ForEach<R, T0, T1>(this ComponentSystem @this, F_RDD<R, T0, T1> operate, in R context, ComponentGroup group)
		where T0 : struct, IComponentData
		where T1 : struct, IComponentData
	{
		ArchetypeChunkComponentType<T0> chunkComponentType0 = @this.GetArchetypeChunkComponentType<T0>(false);
		ArchetypeChunkComponentType<T1> chunkComponentType1 = @this.GetArchetypeChunkComponentType<T1>(false);

		using (NativeArray<ArchetypeChunk> chunks = group.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				int length = chunk.Count;
				void* array0 = chunk.GetNativeArray(chunkComponentType0).GetUnsafePtr();
				void* array1 = chunk.GetNativeArray(chunkComponentType1).GetUnsafePtr();
				for (int i = 0; i < length; i++)
				{
					ref T0 c0 = ref UnsafeUtilityEx.ArrayElementAsRef<T0>(array0, i);
					ref T1 c1 = ref UnsafeUtilityEx.ArrayElementAsRef<T1>(array1, i);
					operate(in context, ref c0, ref c1);
				}
			}
		}
	}

	public delegate void F_WEDD<W, T0, T1>(ref W context, Entity entity, ref T0 c0, ref T1 c1)
		where T0 : struct, IComponentData
		where T1 : struct, IComponentData;

	public static unsafe void ForEach<W, T0, T1>(this ComponentSystem @this, F_WEDD<W, T0, T1> operate, ref W context, ComponentGroup group)
		where T0 : struct, IComponentData
		where T1 : struct, IComponentData
	{
		ArchetypeChunkEntityType entityType = @this.GetArchetypeChunkEntityType();
		ArchetypeChunkComponentType<T0> chunkComponentType0 = @this.GetArchetypeChunkComponentType<T0>(false);
		ArchetypeChunkComponentType<T1> chunkComponentType1 = @this.GetArchetypeChunkComponentType<T1>(false);

		using (NativeArray<ArchetypeChunk> chunks = group.CreateArchetypeChunkArray(Allocator.TempJob))
		{
			foreach (ArchetypeChunk chunk in chunks)
			{
				int length = chunk.Count;
				NativeArray<Entity> entities = chunk.GetNativeArray(entityType);
				void* array0 = chunk.GetNativeArray(chunkComponentType0).GetUnsafePtr();
				void* array1 = chunk.GetNativeArray(chunkComponentType1).GetUnsafePtr();
				for (int i = 0; i < length; i++)
				{
					ref T0 c0 = ref UnsafeUtilityEx.ArrayElementAsRef<T0>(array0, i);
					ref T1 c1 = ref UnsafeUtilityEx.ArrayElementAsRef<T1>(array1, i);
					operate(ref context, entities[i], ref c0, ref c1);
				}
			}
		}
	}
}