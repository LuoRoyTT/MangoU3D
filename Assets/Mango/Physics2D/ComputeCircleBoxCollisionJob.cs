using Mango.BVH;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    struct ComputeCircleBoxCollisionJob : IJobParallelFor
    {
        public NativeQueue<CollisionPair> CircleBoxCollisionPairs;
        public EntityManager entityManager;
        public void Execute(int i)
        {

        }
    }
}