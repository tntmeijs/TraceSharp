namespace PathTracer.Math
{
    static class Functions
    {
        /// <summary>
        /// Clamp a value between "min" and "max"
        /// </summary>
        /// <param name="value">Value to clamp</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Clamped value</returns>
        public static double Clamp(double value, double min, double max)
        {
            return System.Math.Min(System.Math.Max(value, min), max);
        }

        /// <summary>
        /// Clamp a value between 0.0d and 1.0d
        /// </summary>
        /// <param name="value">Value to clamp</param>
        /// <returns>Clamped value</returns>
        public static double Clamp01(double value)
        {
            return Clamp(value, 0.0d, 1.0d);
        }
    }
}
