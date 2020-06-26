using System;
using System.IO;

namespace PathTracer.Rendering
{
    class Image
    {
        private readonly Color[] Data;

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

        /// <summary>
        /// Save the pixel data to a .ppm file
        /// Overrides any existing files with the same name in the target location
        /// The changes over overwriting an existing file are slim to none as each file uses a Unix timestamp as a name
        /// If the target directory does not exist, the necessary directory structure will be created
        /// 
        /// Format reference: https://en.wikipedia.org/wiki/Netpbm
        /// </summary>
        /// <param name="saveDirectory">Location to save the image to</param>
        /// <param name="fileName">Name that should be used to save the image to disk</param>
        /// <returns>True when the image was saved to disk successfully, false otherwise</returns>
        public bool SaveToPpm(string saveDirectory, string fileName)
        {
            // Ensure the save directory exists
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            // Path to the file
            string fullPath = Path.GetFullPath(Path.Combine(saveDirectory, fileName));

            // Remove any existing extensions
            fullPath = Path.ChangeExtension(fullPath, null);

            // Add the .ppm extension back
            fullPath = Path.ChangeExtension(fullPath, "ppm");

            // Debug messaging to make the user aware of the save location
            Console.WriteLine("[INFO] Image will be written to: " + fullPath);

            try
            {
                // Create a new file or overwrite an existing file
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    // Magic number
                    writer.WriteLine("P3");

                    // Store image dimensions
                    writer.WriteLine(Width + " " + Height);

                    // Each color channel has a maximum value of 255
                    writer.WriteLine(255);

                    // Write image data
                    for (int i = 0; i < Data.Length; ++i)
                    {
                        // Convert color from normalized range to 0 - 255
                        Color color = Data[i];
                        byte red     = (byte)System.Math.Floor(color.R * 255.0d);
                        byte green   = (byte)System.Math.Floor(color.G * 255.0d);
                        byte blue    = (byte)System.Math.Floor(color.B * 255.0d);

                        // Convert RGB triplets to a string representation
                        string line = red + " " + green + " " + blue;

                        // Save the data
                        writer.WriteLine(line);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR] Exception occurred while writing to ppm file: " + e.Message);
                return false;
            }

            return true;
        }
    }
}
