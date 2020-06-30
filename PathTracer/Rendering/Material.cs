using PathTracer.Math;

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
        /// Specular highlight color
        /// </summary>
        public Color Specular { get; private set; }

        /// <summary>
        /// Strength / intensity of the emissive color
        /// </summary>
        public double EmissiveStrength { get; private set; }

        /// <summary>
        /// Determines how much specular light will be reflected specularly
        /// </summary>
        public double Specularness { get; private set; }

        /// <summary>
        /// Determines how blurry (rough) the reflection will be
        /// </summary>
        public double Roughness { get; private set; }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="albedo">Albedo color</param>
        /// <param name="emissive">Emissive color</param>
        /// <param name="specular">Specular color</param>
        /// <param name="roughness">Surface roughness (0.0d - 1.0d)</param>
        /// <param name="specularness">Surface specularness (0.0d - 1.0d)</param>
        /// <param name="emissiveStrength">Emissive color strength / intensity</param>
        public Material(Color albedo, Color emissive, Color specular, double roughness = 0.0d, double specularness = 1.0d, double emissiveStrength = 0.0d)
        {
            Albedo              = albedo;
            Emissive            = emissive;
            Specular            = specular;

            Roughness           = Functions.Clamp01(roughness);
            Specularness        = Functions.Clamp01(specularness);
            EmissiveStrength    = emissiveStrength;
        }
    }
}
