namespace PathTracer.Math
{
    class Ray
    {
        /// <summary>
        /// Ray start coordinate
        /// </summary>
        public Vector3 Origin { get; }

        /// <summary>
        /// Direction of the ray
        /// </summary>
        public Vector3 Direction { get; }

        /// <summary>
        /// Create a new ray that starts at (0, 0, 0) points along the positive Z axis (0, 0, 1)
        /// </summary>
        public Ray()
        {
            Origin = Vector3.Zero;
            Direction = new Vector3(0.0d, 0.0d, 1.0d);
        }

        /// <summary>
        /// Create a new ray that starts at the specified origin and points into the specified direction
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin      = origin;
            Direction   = direction.Normalized;
        }
    }
}
