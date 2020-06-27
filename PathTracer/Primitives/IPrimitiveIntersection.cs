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
        /// <param name="hitInfo">Hit information</param>
        /// <returns>True when the ray intersects the sphere, false otherwise</returns>
        bool TestRayIntersection(Ray ray, double minHitDistance, double maxHitDistance, ref PrimitiveHitInfo hitInfo);
    }
}
