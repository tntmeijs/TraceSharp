using PathTracer.Math;

namespace PathTracer.Primitives
{
    class PrimitiveHitInfo
    {
        /// <summary>
        /// Distance to the hit
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Surface normal
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// Create new intersection information that defaults to no intersection at all
        /// </summary>
        public PrimitiveHitInfo()
        {
            Distance    = 0.0d;
            Normal      = Vector3.Zero;
        }
    }
}
