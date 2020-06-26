namespace PathTracer.Math
{
    /// <summary>
    /// Basic three-dimensional vector arithmetic
    /// </summary>
    class Vector3
    {
        /// <summary>
        /// X component of the vector
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y component of the vector
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z component of the vector
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Create a new Vector3 with all components set to zero
        /// </summary>
        public static Vector3 Zero => new Vector3(0.0d, 0.0d, 0.0d);

        /// <summary>
        /// Compute the magnitude of this vector
        /// </summary>
        /// <returns>Magnitude (a.k.a. length) of this vector</returns>
        public double Magnitude => System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

        /// <summary>
        /// Create a new Vector3 with all components set to a specified value
        /// </summary>
        /// <param name="x">X component of the vector</param>
        /// <param name="y">Y component of the vector</param>
        /// <param name="z">Z component of the vector</param>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Inverts all values in the vector
        /// </summary>
        /// <param name="A">Input vector</param>
        /// <returns>New vector with all components set to A *= -1</returns>
        public static Vector3 operator -(Vector3 A) => new Vector3(-A.X, -A.Y, -A.Z);

        /// <summary>
        /// Does nothing with a vector, simply keeps all numbers the same
        /// </summary>
        /// <param name="A">Input vector</param>
        /// <returns>Input vector</returns>
        public static Vector3 operator +(Vector3 A) => A;

        /// <summary>
        /// Subtracts the two vectors
        /// </summary>
        /// <param name="lhs">Input vector on the left hand side</param>
        /// <param name="rhs">Input evctor on the right hand side</param>
        /// <returns>Subtracted vector</returns>
        public static Vector3 operator -(Vector3 lhs, Vector3 rhs) => new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);

        /// <summary>
        /// Sums the two vectors
        /// </summary>
        /// <param name="lhs">Input vector on the left hand side</param>
        /// <param name="rhs">Input evctor on the right hand side</param>
        /// <returns>Summed vector</returns>
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs) => new Vector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);

        /// <summary>
        /// Multiplies the two vectors
        /// </summary>
        /// <param name="lhs">Input vector on the left hand side</param>
        /// <param name="rhs">Input evctor on the right hand side</param>
        /// <returns>Multiplied vector</returns>
        public static Vector3 operator *(Vector3 lhs, Vector3 rhs) => new Vector3(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);

        /// <summary>
        /// Calculate the dot product between two vectors
        /// </summary>
        /// <param name="A">Input vector A</param>
        /// <param name="B">Input vector B</param>
        /// <returns>Dot product between two vectors</returns>
        public static double Dot(Vector3 A, Vector3 B) => (A.X * B.X) + (A.Y * B.Y) + (A.Z * B.Z);

        /// <summary>
        /// Calculate the cross product between two vectors
        /// </summary>
        /// <param name="A">Input vector A</param>
        /// <param name="B">Input vector B</param>
        /// <returns>Cross product between two vectors</returns>
        public static Vector3 Cross(Vector3 A, Vector3 B) => new Vector3((A.Y * B.Z) - (A.Z * B.Y), (A.Z * B.X) - (A.X * B.Z), (A.X * B.Y) - (A.Y * B.X));
    }
}
