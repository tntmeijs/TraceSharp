using PathTracer.Math;

namespace PathTracer.Rendering
{
    class Image
    {
        private Color[] Data;

        /// <summary>
        /// Horizontal resolution in pixels
        /// </summary>
        public int Width;

        /// <summary>
        /// Vertical resolution in pixels
        /// </summary>
        public int Height;

        /// <summary>
        /// Create a new image
        /// </summary>
        /// <param name="width">Horizontal resolution</param>
        /// <param name="height">Vertical resolution</param>
        public Image(int width, int height)
        {
            Width   = width;
            Height  = height;

            // Allocate a new image
            Data = new Color[Width * Height];
        }
    }
}
