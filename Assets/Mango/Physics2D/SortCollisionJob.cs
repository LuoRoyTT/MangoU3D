using Mango.BVH;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    struct SortCollisionJob : IJobParallelFor
    {
        public NativeQueue<CollisionPair> CollisionPairsQueue;
        public NativeQueue<CollisionPair> CircleCircleCollisionPairs;
        public NativeQueue<CollisionPair> CircleBoxCollisionPairs;
        public NativeQueue<CollisionPair> BoxBoxCollisionPairs;
        public EntityManager entityManager;
        public void Execute(int i)
        {
            CollisionPair pair = CollisionPairsQueue.Dequeue();
            var boxA =  entityManager.HasComponent<Box2DColliderComponent>(pair.ColliderEntityA);
            var circleA = entityManager.HasComponent<CircleColliderComponent>(pair.ColliderEntityA);
            var boxB = entityManager.HasComponent<Box2DColliderComponent>(pair.ColliderEntityB);
            var circleB = entityManager.HasComponent<CircleColliderComponent>(pair.ColliderEntityB);
            if(boxA && boxB)
            {
                BoxBoxCollisionPairs.Enqueue(pair);
            }
            else if(circleA && circleB)
            {
                CircleCircleCollisionPairs.Enqueue(pair);
            }
            else
            {
                CircleBoxCollisionPairs.Enqueue(pair);
            }
        }
    }
}

