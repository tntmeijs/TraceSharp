using PathTracer.Math;

namespace PathTracer.Rendering
{
    class Color
    {
        private readonly Vector3 Data;

        /// <summary>
        /// Get the red color component
        /// </summary>
        public double R => Data.X;

        /// <summary>
        /// Get the green color component
        /// </summary>
        public double G => Data.Y;

        /// <summary>
        /// Get the blue color component
        /// </summary>
        public double B => Data.Z;

        /// <summary>
        /// Create a red color
        /// </summary>
        public static Color Red => new Color(1.0d, 0.0d, 0.0d);

        /// <summary>
        /// Create a green color
        /// </summary>
        public static Color Green => new Color(0.0d, 1.0d, 0.0d);

        /// <summary>
        /// Create a blue color
        /// </summary>
        public static Color Blue => new Color(0.0d, 0.0d, 1.0d);

        /// <summary>
        /// Create a purple color
        /// </summary>
        public static Color Purple => new Color(1.0d, 0.0d, 1.0d);

        /// <summary>
        /// Create a white color
        /// </summary>
        public static Color White => new Color(1.0d, 1.0d, 1.0d);

        /// <summary>
        /// Create a black color
        /// </summary>
        public static Color Black => new Color(0.0d, 0.0d, 0.0d);

        /// <summary>
        /// Create a new pixel set to black
        /// </summary>
        public Color()
        {
            Data = Vector3.Zero;
        }

        /// <summary>
        /// Creates a new pixel from a Vector3 with a specified color
        /// </summary>
        /// <param name="data"></param>
        public Color(Vector3 data)
        {
            Data = data;
        }

        /// <summary>
        /// Create a new pixel from individual components with a specified color
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        public Color(double red, double green, double blue)
        {
            Data = new Vector3(red, green, blue);
        }

        /// <summary>
        /// Multiply the color with a scalar
        /// </summary>
        /// <param name="lhs">Color to multiply</param>
        /// <param name="scalar">Scalar</param>
        /// <returns>Scaled color</returns>
        public static Color operator *(Color lhs, double scalar) => new Color(lhs.Data * scalar);

        /// <summary>
        /// Multiply two colors
        /// </summary>
        /// <param name="lhs">Left hand side color</param>
        /// <param name="rhs">Right hand side color</param>
        /// <returns>Multiplied color</returns>
        public static Color operator *(Color lhs, Color rhs) => new Color(lhs.Data * rhs.Data);

        /// <summary>
        /// Add two colors
        /// </summary>
        /// <param name="lhs">Left hand side color</param>
        /// <param name="rhs">Right hand side color</param>
        /// <returns>Added colors</returns>
        public static Color operator +(Color lhs, Color rhs) => new Color(lhs.Data + rhs.Data);

        /// <summary>
        /// Linearly interpolate between two colors
        /// </summary>
        /// <param name="A">Color A</param>
        /// <param name="B">Color B</param>
        /// <param name="t">Interpolation factor</param>
        /// <returns>Interpolated color</returns>
        public static Color Mix(Color A, Color B, double t) => new Color(Vector3.Lerp(A.Data, B.Data, t));

        /// <summary>
        /// Apply gamma correction to the color
        /// Reference: https://learnopengl.com/Advanced-Lighting/Gamma-Correction
        /// </summary>
        /// <param name="color">Linear color</param>
        /// <returns>Gamma-corrected color</returns>
        public static Color GammaCorrection(Color color, double gamma = 2.2d)
        {
            double powR = System.Math.Pow(color.R, 1.0d / gamma);
            double powG = System.Math.Pow(color.G, 1.0d / gamma);
            double powB = System.Math.Pow(color.B, 1.0d / gamma);

            return new Color(powR, powG, powB);
        }

        /// <summary>
        /// Tone mapping
        /// https://knarkowicz.wordpress.com/2016/01/06/aces-filmic-tone-mapping-curve/
        /// </summary>
        /// <param name="color">Color to apply ACES tone map to</param>
        /// <returns>Tone mapped color</returns>
        public static Color ToneMapACES(Color color)
        {
            double a = 2.51d;
            double b = 0.03d;
            double c = 2.43d;
            double d = 0.59d;
            double e = 0.14d;
            
            double clampedR = Functions.Clamp01((color.R * (a * color.R + b)) / (color.R * (c * color.R + d) + e));
            double clampedG = Functions.Clamp01((color.G * (a * color.G + b)) / (color.G * (c * color.G + d) + e));
            double clampedB = Functions.Clamp01((color.B * (a * color.B + b)) / (color.B * (c * color.B + d) + e));

            return new Color(clampedR, clampedG, clampedB);
        }

        /// <summary>
        /// Check if two colors are the same
        /// </summary>
        /// <param name="obj">Color to check against</param>
        /// <returns>True when equal colors, false when one of the components is different</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Color colObj = obj as Color;
            return Data.Equals(colObj.Data);
        }

        /// <summary>
        /// Override GetHashCode(), not implemented
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
