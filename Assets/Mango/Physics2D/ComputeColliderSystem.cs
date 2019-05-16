using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine.Experimental.LowLevel;
using Unity.Burst;
using Mango.BVH;

namespace Mango.Physics2D
{
    public struct CollisionPair
    {
        public Entity ColliderEntityA;
        public Entity ColliderEntityB;
    }

    [DisableAutoCreation]
    public class ComputeColliderSystem : JobComponentSystem
    {
        [Inject]
        private ColliderGroup colliderGroup;
        public NativeQueue<CollisionPair> CollisionPairsQueue;
        public NativeQueue<CollisionPair> CircleCircleCollisionPairs;
        public NativeQueue<CollisionPair> CircleBoxCollisionPairs;
        public NativeQueue<CollisionPair> BoxBoxCollisionPairs;
        private BVHConstructor _BVH;
        private int Capacity;
        protected override void OnCreateManager()
        {
            Capacity = 0;

            _BVH = new BVHConstructor(Capacity);

        }
        protected override void OnDestroyManager()
        {
            _BVH.Dispose();
        }
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var aabbs = colliderGroup.AABBs;
            inputDeps = _BVH.Calculate(inputDeps, colliderGroup);
            BuildColliderPairsJob buildColliderPairsJob = new BuildColliderPairsJob
            {
                BVHArray = _BVH.BVHArray,
                CollisionPairsQueue = CollisionPairsQueue,
            };
            inputDeps = buildColliderPairsJob.Schedule(colliderGroup.Length,64,inputDeps);
            SortCollisionJob sortCollisionJob = new SortCollisionJob
            {
                entityManager = EntityManager,
                CollisionPairsQueue = CollisionPairsQueue,
                CircleCircleCollisionPairs = CircleCircleCollisionPairs,
                CircleBoxCollisionPairs = CircleBoxCollisionPairs,
                BoxBoxCollisionPairs = BoxBoxCollisionPairs,
            };
            inputDeps = sortCollisionJob.Schedule(CollisionPairsQueue.Count,64,inputDeps);

            ComputeBoxBoxCollisionJob computeBoxBoxCollisionJob = new ComputeBoxBoxCollisionJob
            {
                entityManager = EntityManager,
                BoxBoxCollisionPairs = BoxBoxCollisionPairs,
            };

            ComputeCircleCircleCollisionJob computeCircleCircleCollisionJob = new ComputeCircleCircleCollisionJob
            {
                entityManager = EntityManager,
                CircleCircleCollisionPairs = CircleCircleCollisionPairs,
            };
            ComputeCircleBoxCollisionJob computeCircleBoxCollisionJob = new ComputeCircleBoxCollisionJob
            {
                entityManager = EntityManager,
                CircleBoxCollisionPairs = CircleBoxCollisionPairs,
            };


            return inputDeps;
        }
    }
}
