using PathTracer.Math;

namespace PathTracer.Primitives
{
    /// <summary>
    /// Interface to test ray - primitive intersections
    /// </summary>
    interface IPrimitiveIntersectTest
    {
        /// <summary>
        /// Tests whether the ray could intersect with this primitive
        /// </summary>
        /// <param name="ray">Ray to test against this primitive</param>
        /// <returns>True on intersection, false otherwise</returns>
        bool Intersects(Ray ray);
    }
}
