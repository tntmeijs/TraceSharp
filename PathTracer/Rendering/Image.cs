using System;

namespace PathTracer.Rendering
{
    class Image
    {
        private Color[] Data;

        /// <summary>
        /// Horizontal resolution in pixels
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Vertical resolution in pixels
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Create a new image with all pixels set to black
        /// </summary>
        /// <param name="width">Horizontal resolution</param>
        /// <param name="height">Vertical resolution</param>
        public Image(int width, int height)
        {
            Width   = width;
            Height  = height;

            // Allocate a new image
            Data = new Color[Width * Height];

            // Make the image black
            for (int i = 0; i < Data.Length; ++i)
            {
                Data[i] = Color.Black;
            }
        }

        /// <summary>
        /// Set the specified pixel's color
        /// </summary>
        /// <param name="index">Index of the pixel to set</param>
        /// <param name="color">Color to set the pixel to</param>
        public void SetPixel(int index, Color color)
        {
            if (index < 0 || index >= Data.Length)
            {
                Console.WriteLine("[ERROR] Image pixel index out of bounds.");
            }

            Data[index] = color;
        }

        /// <summary>
        /// Get the specified pixel's color
        /// </summary>
        /// <param name="index">Index of the pixel to retrieve the color from</param>
        /// <returns>Pixel's color</returns>
        public Color GetPixel(int index)
        {
            if (index < 0 || index >= Data.Length)
            {
                Console.WriteLine("[ERROR] Image pixel index out of bounds.");
            }

            return Data[index];
        }
    }
}
