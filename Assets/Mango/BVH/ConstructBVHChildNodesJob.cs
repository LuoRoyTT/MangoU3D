using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Mango.BVH
{
    [BurstCompile]
    struct ConstructBVHChildNodesJob : IJobParallelFor
    {
        [ReadOnly] public ComponentDataArray<BVHAABB> AABB;

        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<int> indexConverter;
        
        [WriteOnly]
        public NativeArray<BVHNode> BVHArray;
        public EntityArray Entities;

        public void Execute(int i)
        {
            int halfLength = BVHArray.Length / 2;
            if (i < halfLength) return;
            int localId = i - halfLength;
            if (localId >= AABB.Length) return;
            int entityindex = indexConverter[i - halfLength];
            BVHNode bvhNode = new BVHNode();
            bvhNode.aabb = AABB[entityindex];
            bvhNode.EntityId = Entities[entityindex];
            bvhNode.RightNodeIndex = i;
            bvhNode.LeftNodeIndex = -1;
            bvhNode.IsValid = 2;
            BVHArray[i] = bvhNode;
        }
    }
}