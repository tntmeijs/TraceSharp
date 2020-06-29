using PathTracer.Math;
using PathTracer.Rendering;

namespace PathTracer.Primitives
{
    abstract class PrimitiveBase : IPrimitiveIntersection
    {
        /// <summary>
        /// Material to use when rendering this object
        /// </summary>
        public Material Material;

        /// <summary>
        /// Create a new primitive and assign a material to be used while rendering
        /// </summary>
        /// <param name="material">Material to render the primitive with</param>
        public PrimitiveBase(Material material)
        {
            Material = material;
        }

        /// <summary>
        /// IPrimitiveIntersection implementation
        /// Needs to be overwritten in derived classes
        /// </summary>
        /// <param name="ray">Ray to check against the primitive</param>
        /// <param name="minHitDistance">Minimum distance a ray needs to travel before it can hit a primitive</param>
        /// <param name="maxHitDistance">Maximum distance a ray can travel until it is considered a miss</param>
        /// <returns>Surface hit information</returns>
        public abstract PrimitiveHitInfo TestRayIntersection(Ray ray, double minHitDistance, double maxHitDistance);
    }
}
