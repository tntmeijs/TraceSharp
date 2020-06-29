using System;
using System.Collections.Generic;

using PathTracer.Configuration;
using PathTracer.Math;
using PathTracer.Primitives;

namespace PathTracer.Rendering
{
    class Renderer
    {
        private readonly double MinRayLength;
        private readonly double MaxRayLength;
        private readonly double Exposure;
        private readonly double FieldOfView;

        private readonly int MaxBounces;
        private readonly int SamplesPerPixel;
        private readonly int OutputWidth;
        private readonly int OutputHeight;

        private readonly string SaveDirectory;
        private readonly string FileName;

        private readonly Image OutputImage;

        private readonly Random RandomNumberGenerator;

        /// <summary>
        /// Scene containing all primitives to trace against
        /// </summary>
        private List<PrimitiveBase> Scene;

        /// <summary>
        /// Construct a new renderer
        /// </summary>
        public Renderer()
        {
            // Retrieve ray configuration information
            MinRayLength    = ConfigurationParser.GetDouble("MIN_RAY_LENGTH");
            MaxRayLength    = ConfigurationParser.GetDouble("MAX_RAY_LENGTH");
            MaxBounces      = ConfigurationParser.GetInt("MAX_BOUNCES");
            SamplesPerPixel = ConfigurationParser.GetInt("SAMPLES_PER_PIXEL");

            // Retrieve camera configuration information
            Exposure        = ConfigurationParser.GetDouble("EXPOSURE");
            FieldOfView     = ConfigurationParser.GetDouble("FIELD_OF_VIEW");

            // Retrieve image output configuration information
            OutputWidth     = ConfigurationParser.GetInt("IMAGE_WIDTH");
            OutputHeight    = ConfigurationParser.GetInt("IMAGE_HEIGHT");
            SaveDirectory   = ConfigurationParser.GetString("SAVE_DIRECTORY");
            FileName        = ConfigurationParser.GetString("FILE_NAME");

            OutputImage = new Image(OutputWidth, OutputHeight);

            RandomNumberGenerator = new Random();

            Scene = new List<PrimitiveBase>();
        }

        /// <summary>
        /// Add a new primitive to the scene
        /// </summary>
        /// <param name="primitive">Primitive to trace against</param>
        public void AddPrimitiveToScene(PrimitiveBase primitive)
        {
            Scene.Add(primitive);
        }

        /// <summary>
        /// Start rendering the scene
        /// </summary>
        public void Start()
        {
            int pixelIndex = 0;

            for (int vInt = 0; vInt < OutputHeight; ++vInt)
            {
                Color[] pixelData = ProcessLine(vInt);

                // Save the line data in the final image
                foreach (Color pixel in pixelData)
                {
                    // Post processing steps
                    Color finalColor = ApplyPostProcessing(pixel);

                    // Store the result
                    OutputImage.SetPixel(pixelIndex++, finalColor);
                }
            }
        }

        /// <summary>
        /// Save the image data to disk as a .ppm file
        /// </summary>
        /// <returns>True when saved successfully, false on failure</returns>
        public bool SaveToDisk()
        {
            return OutputImage.SaveToPpm(SaveDirectory, FileName);
        }

        /// <summary>
        /// Apply post processing to the pixel's color
        /// </summary>
        /// <param name="color">Input color</param>
        /// <returns>Processed color</returns>
        public Color ApplyPostProcessing(Color color)
        {
            Color output = color;

            output *= Exposure;
            output = Color.ToneMapACES(output);
            output = Color.GammaCorrection(output);

            return output;
        }

        /// <summary>
        /// Process a single line of the output picture
        /// </summary>
        /// <param name="lineIndex">Row number of the line to process</param>
        /// <returns>Array of pixel colors</returns>
        public Color[] ProcessLine(int lineIndex)
        {
            Color[] output = new Color[OutputWidth];

            for (int uInt = 0; uInt < OutputWidth; ++uInt)
            {
                // FOV to camera distance
                double cameraDistance = 1.0d / System.Math.Tan(FieldOfView * 0.5d * System.Math.PI / 180.0d);

                // Trace the scene for this pixel
                Color outputColor = Color.Black;
                Color previousFrameColor = Color.White;

                for (int i = 0; i < SamplesPerPixel; ++i)
                {
                    // Sub-pixel jitter for anti-aliasing
                    double subPixelJitterU = RandomNumberGenerator.NextDouble() - 0.5d;
                    double subPixelJitterV = RandomNumberGenerator.NextDouble() - 0.5d;

                    // Normalized UV coordinates
                    double u = (uInt + subPixelJitterU) / OutputWidth;
                    double v = (lineIndex + subPixelJitterV) / OutputHeight;

                    // Invert the vertical range to flip the image to the correct orientation
                    v = 1.0d - v;

                    // Transform from 0 - 1 to -1 - 1
                    u = (u * 2.0d) - 1.0d;
                    v = (v * 2.0d) - 1.0d;

                    // Correct for the aspect ratio
                    double aspectRatio = (double)OutputWidth / OutputHeight;
                    v /= aspectRatio;

                    // Ray starts at the camera origin and goes through the imaginary pixel rectangle
                    Ray cameraRay = new Ray(Vector3.Zero, new Vector3(u, v, cameraDistance));

                    Color color = TracePixel(cameraRay);

                    // Average the color over the number of samples
                    // Each sample has less impact on the final image than the previous sample
                    outputColor = Color.Mix(previousFrameColor, color, 1.0d / (i + 1));
                    previousFrameColor = outputColor;
                }

                output[uInt] = outputColor;
            }

            return output;
        }

        public Color TracePixel(Ray ray)
        {
            Color color = Color.Black;
            Color throughput = Color.White;

            // Epsilon is used to prevent the new ray origin from starting inside the surface
            const double EPSILON = 0.01d;

            Vector3 rayPos = ray.Origin;
            Vector3 rayDir = ray.Direction;

            for (int i = 0; i < MaxBounces; ++i)
            {
                PrimitiveHitInfo hitInfo = new PrimitiveHitInfo();
                hitInfo.Distance = MaxRayLength;

                // Trace against the scene
                foreach (PrimitiveBase primitive in Scene)
                {
                    if (primitive.TestRayIntersection(new Ray(rayPos, rayDir), MinRayLength, MaxRayLength, ref hitInfo))
                    {
                        hitInfo.Albedo = primitive.Material.Albedo;
                        hitInfo.Emissive = primitive.Material.Emissive * primitive.Material.EmissiveStrength;
                    }
                }

                // Ray missed
                if (hitInfo.Distance == MaxRayLength)
                {
                    break;
                }

                // Construct a new ray
                rayPos = (rayPos + rayDir * hitInfo.Distance) + hitInfo.Normal * EPSILON;
                rayDir = hitInfo.Normal + Functions.RandomUnitVector(RandomNumberGenerator);

                // Add emissive lighting
                color += hitInfo.Emissive * throughput;

                // When a ray bounces off a surface, all future lighting for that ray is multiplied by the color of that surface
                throughput *= hitInfo.Albedo;
            }

            return color;
        }
    }
}
