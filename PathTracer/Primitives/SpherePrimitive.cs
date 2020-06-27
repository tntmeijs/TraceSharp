using PathTracer.Math;

namespace PathTracer.Primitives
{
    /// <summary>
    /// Reference: https://viclw17.github.io/2018/07/16/raytracing-ray-sphere-intersection/
    /// </summary>
    class SpherePrimitive : IPrimitiveIntersection
    {
        /// <summary>
        /// Center of the sphere in 3D space
        /// </summary>
        public Vector3 Center { get; set; }

        /// <summary>
        /// Radius of the sphere
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Create a new sphere primitive at (0, 0, 0) with a radius of 1
        /// </summary>
        public SpherePrimitive()
        {
            Center = Vector3.Zero;
            Radius = 1.0d;
        }

        /// <summary>
        /// Create a new sphere primitive with a specified center and radius
        /// </summary>
        /// <param name="center">Origin to place the sphere at</param>
        /// <param name="radius">Radius of the sphere</param>
        public SpherePrimitive(Vector3 center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Determine whether the ray intersects with the sphere
        /// </summary>
        /// <param name="ray">Ray to test against</param>
        /// <param name="minHitDistance">Minimum distance from the ray's origin before a hit is considered</param>
        /// <param name="maxHitDistance">Maximum distance from the ray's origin before a miss is considered</param>
        /// <param name="hitInfo">Hit information</param>
        /// <returns>True when the ray intersects the sphere, false otherwise</returns>
        public bool TestRayIntersection(Ray ray, double minHitDistance, double maxHitDistance, out PrimitiveHitInfo hitInfo)
        {
            hitInfo = new PrimitiveHitInfo();

            // Account for the sphere's radius
            minHitDistance += Radius;
            maxHitDistance += Radius;

            // Quadratic formula, hence the names a, b, c, d
            Vector3 originCenter = ray.Origin - Center;
            double a = Vector3.Dot(ray.Direction, ray.Direction);
            double b = 2.0d * Vector3.Dot(originCenter, ray.Direction);
            double c = Vector3.Dot(originCenter, originCenter) - Radius * Radius;
            double d = (b * b) - (4.0d * a * c);

            if (d >= 0.0d)
            {
                // Negative test of the quadratic formula
                double numerator = -b - System.Math.Sqrt(d);
                if (numerator > 0.0d)
                {
                    hitInfo.Distance = numerator / (2.0d * a);

                    // Too close / too far
                    if (hitInfo.Distance < minHitDistance || hitInfo.Distance > maxHitDistance)
                    {
                        return false;
                    }

                    // Hit
                    return true;
                }

                // Positive test of the quadratic formula
                numerator = -b + System.Math.Sqrt(d);
                if (numerator > 0.0d)
                {
                    hitInfo.Distance = numerator / (2.0d * a);

                    // Too close / too far
                    if (hitInfo.Distance < minHitDistance || hitInfo.Distance > maxHitDistance)
                    {
                        return false;
                    }

                    // Hit
                    return true;
                }
            }

            // No intersections
            return false;
        }
    }
}
