using Unity.Entities;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    public struct TransformComponent : IComponentData
    {
        public float3 Position;
        public float3 Rotation;
        public float3 Forward;
    }
}