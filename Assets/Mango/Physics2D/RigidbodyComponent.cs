using Unity.Entities;
using Unity.Mathematics;

namespace Mango.Physics2D
{
    public struct RigidbodyComponent : IComponentData
    {
	    public float3 Velocity;
	    public float3 AngularVelocity;
        public float Mass;
        public bool IgnoreGravity;
    }
}
