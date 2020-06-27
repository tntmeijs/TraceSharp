using PathTracer.Math;
using PathTracer.Rendering;

namespace PathTracer.Primitives
{
    class SpherePrimitive : PrimitiveBase
    {
        /// <summary>
        /// Center of the sphere in 3D space
        /// </summary>
        public Vector3 Center;

        /// <summary>
        /// Radius of the sphere
        /// </summary>
        public double Radius;

        /// <summary>
        /// Create a new sphere primitive at (0, 0, 0) with a radius of 1
        /// </summary>
        /// <param name="material">Material to render this sphere with</param>
        public SpherePrimitive(Material material) : base(material)
        {
            Center = Vector3.Zero;
            Radius = 1.0d;
        }

        /// <summary>
        /// Create a new sphere primitive with a specified center and radius
        /// </summary>
        /// <param name="center">Origin to place the sphere at</param>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="material">Material to render this sphere with</param>
        public SpherePrimitive(Vector3 center, double radius, Material material) : base(material)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Determine whether the ray intersects with the sphere
        /// Reference: Shadertoy by Alan Wolfe
        /// </summary>
        /// <param name="ray">Ray to test against</param>
        /// <param name="minHitDistance">Minimum distance from the ray's origin before a hit is considered</param>
        /// <param name="maxHitDistance">Maximum distance from the ray's origin before a miss is considered</param>
        /// <param name="hitInfo">Hit information</param>
        /// <returns>True when the ray intersects the sphere, false otherwise</returns>
        public override bool TestRayIntersection(Ray ray, double minHitDistance, double maxHitDistance, ref PrimitiveHitInfo hitInfo)
        {
            // Vector from center of the sphere to where the ray begins
            Vector3 m = ray.Origin - Center;

            double b = Vector3.Dot(m, ray.Direction);
            double c = Vector3.Dot(m, m) - (Radius * Radius);

            if (c > 0.0d && b > 0.0d)
            {
                return false;
            }

            // Discriminant (quadratic formula)
            double d = (b * b) - c;

            // Ray misses the sphere
            if (d < 0.0d)
            {
                return false;
            }

            // Compute smallest value of intersection
            bool inside = false;
            double distance = -b - System.Math.Sqrt(d);
            
            if (distance < 0.0d)
            {
                inside = true;
                distance = -b + System.Math.Sqrt(d);
            }

            if (distance > minHitDistance && distance < hitInfo.Distance)
            {
                hitInfo.Distance = distance;
                hitInfo.Normal = (ray.Origin + (ray.Direction * distance) - Center) * (inside ? -1.0d : 1.0d);
                return true;
            }

            return false;
        }
    }
}
