using PathTracer.Math;

namespace PathTracer.Rendering
{
    /// <summary>
    /// Represents color data
    /// </summary>
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
