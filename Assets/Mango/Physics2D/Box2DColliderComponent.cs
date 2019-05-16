using Unity.Entities;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    public struct Box2DColliderComponent : IComponentData
    {
        public float2 center; 
        public float2 direction;
        public AABB Rect;
        public bool IsTrigger;
    }
}

