namespace PathTracer.Math
{
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
        /// Create a forward-facing vector
        /// </summary>
        public static Vector3 Forward => new Vector3(0.0d, 0.0d, 1.0d);

        /// <summary>
        /// Create a backward-facing vector
        /// </summary>
        public static Vector3 Backward => -Forward;

        /// <summary>
        /// Create an upward-facing vector
        /// </summary>
        public static Vector3 Up => new Vector3(0.0d, 1.0d, 0.0d);

        /// <summary>
        /// Create a downward-facing vector
        /// </summary>
        public static Vector3 Down => -Up;

        /// <summary>
        /// Create a left-facing vector
        /// </summary>
        public static Vector3 Left => new Vector3(-1.0d, 0.0d, 0.0d);

        /// <summary>
        /// Create a right-facing vector
        /// </summary>
        public static Vector3 Right => -Left;

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
        /// Compute a normalized version of this vector
        /// </summary>
        public Vector3 Normalized
        {
            get
            {
                double magnitude = Magnitude;
                return new Vector3(X / magnitude, Y / magnitude, Z / magnitude);
            }
        }

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
        /// Normalize this vector
        /// </summary>
        public void Normalize()
        {
            double magnitude = Magnitude;
            X /= magnitude;
            Y /= magnitude;
            Z /= magnitude;
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

        /// <summary>
        /// Compare this vector against another vector
        /// </summary>
        /// <param name="obj">Vector to compare against</param>
        /// <returns>True when all components have equal values, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Vector3 vecObj = obj as Vector3;
            return (X == vecObj.X && Y == vecObj.Y && Z == vecObj.Z);
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
