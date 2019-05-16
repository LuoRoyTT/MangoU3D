using Mango.BVH;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    struct ComputeCircleCircleCollisionJob : IJobParallelFor
    {
        public NativeQueue<CollisionPair> CircleCircleCollisionPairs;
        public EntityManager entityManager;
        public void Execute(int i)
        {

        }
    }
}