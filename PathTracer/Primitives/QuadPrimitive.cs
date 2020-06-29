using PathTracer.Math;
using PathTracer.Rendering;

namespace PathTracer.Primitives
{
    class QuadPrimitive : PrimitiveBase
    {
        /// <summary>
        /// Bottom left corner of the quad
        /// </summary>
        public Vector3 BottomLeft   { get; private set; }

        /// <summary>
        /// Bottom right corner of the quad
        /// </summary>
        public Vector3 BottomRight  { get; private set; }

        /// <summary>
        /// Top right corner of the quad
        /// </summary>
        public Vector3 TopRight     { get; private set; }

        /// <summary>
        /// Top left corner of the quad
        /// </summary>
        public Vector3 TopLeft      { get; private set; }

        /// <summary>
        /// Create a new quad primitive with sides of length one
        /// </summary>
        /// <param name="material">Material to render this quad with</param>
        public QuadPrimitive(Material material) : base(material)
        {
            BottomLeft  = new Vector3(-0.5d, -0.5d, 0.0d);
            BottomRight = new Vector3( 0.5d, -0.5d, 0.0d);
            TopRight    = new Vector3( 0.5d,  0.5d, 0.0d);
            TopLeft     = new Vector3(-0.5d,  0.5d, 0.0d);
        }

        /// <summary>
        /// Create a new quad primitive
        /// </summary>
        /// <param name="bottomLeft">Bottom left corner of the quad</param>
        /// <param name="bottomRight">Bottom right corner of the quad</param>
        /// <param name="topRight">Top right corner of the quad</param>
        /// <param name="topLeft">Top left corner of the quad</param>
        /// <param name="material">Material to render this quad with</param>
        public QuadPrimitive(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight, Vector3 topLeft, Material material) : base(material)
        {
            BottomLeft  = bottomLeft;
            BottomRight = bottomRight;
            TopRight    = topRight;
            TopLeft     = topLeft;
        }

        /// <summary>
        /// Determine whether the ray intersects with the sphere
        /// Reference: Shadertoy by Alan Wolfe
        /// </summary>
        /// <param name="ray">Ray to test against</param>
        /// <param name="minHitDistance">Minimum distance from the ray's origin before a hit is considered</param>
        /// <param name="maxHitDistance">Maximum distance from the ray's origin before a miss is considered</param>
        /// <returns>Hit information</returns>
        public override PrimitiveHitInfo TestRayIntersection(Ray ray, double minHitDistance, double maxHitDistance)
        {
            PrimitiveHitInfo hitInfo = new PrimitiveHitInfo()
            {
                Albedo = Material.Albedo,
                Emissive = Material.Emissive * Material.EmissiveStrength
            };

            // Calculate the normal vector
            Vector3 normal = Vector3.Cross(TopRight - BottomLeft, TopRight - BottomRight).Normalized;

            // Flip vertex order if needed
            if (Vector3.Dot(normal, ray.Direction) > 0.0d)
            {
                normal = -normal;

                Vector3 temp = TopLeft;
                TopLeft = BottomLeft;
                BottomLeft = temp;

                temp = BottomRight;
                BottomRight = TopRight;
                TopRight = temp;
            }

            Vector3 p = ray.Origin;
            Vector3 q = ray.Origin + ray.Direction;
            Vector3 pq = q - p;
            Vector3 pa = BottomLeft - p;
            Vector3 pb = BottomRight - p;
            Vector3 pc = TopRight - p;

            // Determine which triangle to test against by testing against the diagonal first
            Vector3 m = Vector3.Cross(pc, pq);
            double v = Vector3.Dot(pa, m);
            Vector3 intersectPosition;

            if (v >= 0.0d)
            {
                // Test against triangle a, b, c
                double u = -Vector3.Dot(pb, m);

                if (u < 0.0d)
                {
                    return hitInfo;
                }

                double w = ScalarTripleProduct(pq, pb, pa);

                if (w < 0.0d)
                {
                    return hitInfo;
                }

                double denominator = 1.0d / (u + v + w);
                u *= denominator;
                v *= denominator;
                w *= denominator;

                intersectPosition = (BottomLeft * u) + (BottomRight * v) + (TopRight * w);
            }
            else
            {
                Vector3 pd = TopLeft - p;

                double u = Vector3.Dot(pd, m);

                if (u < 0.0d)
                {
                    return hitInfo;
                }

                double w = ScalarTripleProduct(pq, pa, pd);

                if (w < 0.0d)
                {
                    return hitInfo;
                }

                v = -v;
                
                double denominator = 1.0d / (u + v + w);
                u *= denominator;
                v *= denominator;
                w *= denominator;

                intersectPosition = (BottomLeft * u) + (TopLeft * v) + (TopRight * w);
            }

            double distance;
            if (System.Math.Abs(ray.Direction.X) > 0.1d)
            {
                distance = (intersectPosition.X - ray.Origin.X) / ray.Direction.X;
            }
            else if (System.Math.Abs(ray.Direction.Y) > 0.1d)
            {
                distance = (intersectPosition.Y - ray.Origin.Y) / ray.Direction.Y;
            }
            else
            {
                distance = (intersectPosition.Z - ray.Origin.Z) / ray.Direction.Z;
            }

            if (distance > minHitDistance && distance < maxHitDistance)
            {
                hitInfo.Distance = distance;
                hitInfo.Normal = normal;
                hitInfo.DidHit = true;
                return hitInfo;
            }

            return hitInfo;
        }

        /// <summary>
        /// Calculate the scalar triple product
        /// Reference: https://mathinsight.org/scalar_triple_product
        /// </summary>
        /// <param name="a">Vector A</param>
        /// <param name="b">Vector B</param>
        /// <param name="c">Vector C</param>
        /// <returns>Scalar triple product</returns>
        private double ScalarTripleProduct(Vector3 a, Vector3 b, Vector3 c)
        {
            return Vector3.Dot(Vector3.Cross(a, b), c);
        }
    }
}
