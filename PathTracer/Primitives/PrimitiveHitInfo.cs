using PathTracer.Math;
using PathTracer.Rendering;

namespace PathTracer.Primitives
{
    class PrimitiveHitInfo
    {
        /// <summary>
        /// Distance to the hit
        /// </summary>
        public double Distance;

        /// <summary>
        /// Surface normal
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Surface color
        /// </summary>
        public Color Albedo;

        /// <summary>
        /// Surface emissiveness
        /// </summary>
        public Color Emissive;

        /// <summary>
        /// Create new intersection information that defaults to no intersection at all
        /// </summary>
        public PrimitiveHitInfo()
        {
            Distance    = 0.0d;
            Normal      = Vector3.Zero;
            Albedo      = Color.Black;
            Emissive    = Color.Black;
        }
    }
}
