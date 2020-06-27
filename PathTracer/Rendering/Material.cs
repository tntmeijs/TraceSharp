namespace PathTracer.Rendering
{
    class Material
    {
        /// <summary>
        /// Albedo color
        /// </summary>
        public Color Albedo { get; private set; }

        /// <summary>
        /// Emissive color
        /// </summary>
        public Color Emissive { get; private set; }

        /// <summary>
        /// Strength / intensity of the emissive color
        /// </summary>
        public double EmissiveStrength { get; private set; }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="albedo">Albedo color</param>
        /// <param name="emissive">Emissive color</param>
        /// <param name="emissiveStrength">Emissive color strength / intensity</param>
        public Material(Color albedo, Color emissive, double emissiveStrength = 0.0d)
        {
            Albedo              = albedo;
            Emissive            = emissive;
            EmissiveStrength    = emissiveStrength;
        }
    }
}
