using Mango.BVH;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    struct BuildColliderPairsJob : IJobParallelFor
    {
        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<BVHNode> BVHArray;

        [NativeDisableParallelForRestriction]
        public NativeQueue<CollisionPair> CollisionPairsQueue;

        public int HalfBVHArrayLength;

        void QueryBVHNode(int comparedToNode, int leafNodeIndex)
        {
            bool is_overlap = Utils.AABBToAABBOverlap(BVHArray[comparedToNode].aabb, BVHArray[leafNodeIndex].aabb);

            if (BVHArray[comparedToNode].IsValid > 0 &&
                is_overlap)
            {
                // leaf node
                if (BVHArray[comparedToNode].LeftNodeIndex < 0)
                {
                    CollisionPair newPair = new CollisionPair
                    {
                        ColliderEntityA = BVHArray[leafNodeIndex].EntityId,
                        ColliderEntityB = BVHArray[comparedToNode].EntityId,
                    };
                    CollisionPairsQueue.Enqueue(newPair);
                }
                else
                {
                    int left = BVHArray[comparedToNode].LeftNodeIndex;
                    int right = BVHArray[comparedToNode].RightNodeIndex;
                    if (left != leafNodeIndex) QueryBVHNode(left, leafNodeIndex);
                    if (right != leafNodeIndex) QueryBVHNode(right, leafNodeIndex);
                }
            }
        }

        public void Execute(int i)
        {
            QueryBVHNode(0, HalfBVHArrayLength + i);
        }
    }
}

