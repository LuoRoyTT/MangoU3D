using Unity.Entities;
using Unity.Mathematics;

namespace Mango.BVH
{
    public struct BVHNode
    {
        public BVHAABB aabb;
        public Entity EntityId;
        public int LeftNodeIndex;
        public int RightNodeIndex;
        public int ParentNodeIndex;
        public byte IsValid;
    }
}