using Unity.Entities;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    public struct CircleColliderComponent : IComponentData
    {
        public float2 center; 
        public float radius;
        public bool IsTrigger;
    }
}

