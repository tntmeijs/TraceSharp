using System;
using PathTracer.Math;
using PathTracer.Rendering;

namespace PathTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            // The path tracer will create images with a resolution of 1280x720
            const int OUTPUT_WIDTH  = 1280;
            const int OUTPUT_HEIGHT = 720;

            // Output images will be saved here
            const string OUTPUT_PATH = "./";
            const string OUTPUT_NAME = "TraceSharp_output";

            Image outputImage = new Image(OUTPUT_WIDTH, OUTPUT_HEIGHT);

            // Calculate a color for every pixel in the image
            int pixelIndex = 0;
            for (int vInt = 0; vInt < OUTPUT_HEIGHT; ++vInt)
            {
                for (int uInt = 0; uInt < OUTPUT_WIDTH; ++uInt)
                {
                    // Normalized UV coordinates
                    double u = (double)uInt / OUTPUT_WIDTH;
                    double v = (double)vInt / OUTPUT_HEIGHT;

                    // Invert the vertical range to flip the image to the correct orientation
                    v = 1.0d - v;

                    // Transform from 0 - 1 to -1 - 1
                    u = (u * 2.0d) - 1.0d;
                    v = (v * 2.0d) - 1.0d;

                    // Ray starts at the camera origin and goes through the imaginary pixel rectangle
                    Ray cameraRay = new Ray(Vector3.Zero, new Vector3(u, v, 1.0d));

                    // Simply use the UV coordinates as the pixel colors
                    Color outputColor = new Color(cameraRay.Direction);
                    outputImage.SetPixel(pixelIndex++, outputColor);
                }
            }

            // Store the result on disk to allow the user to see the results of the path tracer
            if (!outputImage.SaveToPpm(OUTPUT_PATH, OUTPUT_NAME))
            {
                Console.WriteLine("[ERROR] Failed to write ppm file to disk.");
            }
        }
    }
}
