using PathTracer.Math;

namespace PathTracer.Primitives
{
    interface IPrimitiveIntersection
    {
        /// <summary>
        /// Calculate ray - primitive intersection
        /// </summary>
        /// <param name="ray">Ray to test against this primitive</param>
        /// <param name="minHitDistance">Minimum distance from the ray's origin before a hit is considered</param>
        /// <param name="maxHitDistance">Maximum distance from the ray's origin before a miss is considered</param>
        /// <returns>Hit information</returns>
        PrimitiveHitInfo TestRayIntersection(Ray ray, double minHitDistance, double maxHitDistance);
    }
}
