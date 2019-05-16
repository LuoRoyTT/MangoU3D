using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Mango.BVH
{
    [Serializable]
    public struct BVHAABB : IComponentData
    {
        public float3 Min;
        public float3 Max;
    }

    public class AABBComponent : ComponentDataWrapper<BVHAABB> { }
}