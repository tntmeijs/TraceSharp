using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using PathTracer.Configuration;
using PathTracer.Math;
using PathTracer.Primitives;
using System.Threading;
using System.Linq;

namespace PathTracer.Rendering
{
    class Renderer
    {
        /// <summary>
        /// Rendering settings (will be retrieved from App.config)
        /// </summary>
        private readonly double MinRayLength;
        private readonly double MaxRayLength;
        private readonly double Exposure;
        private readonly double FieldOfView;

        private readonly int MaxBounces;
        private readonly int SamplesPerPixel;
        private readonly int OutputWidth;
        private readonly int OutputHeight;

        /// <summary>
        /// When writing image data to disk, this is where it will end up
        /// </summary>
        private readonly string SaveDirectory;
        private readonly string FileName;

        /// <summary>
        /// Image data used as the path tracer's output
        /// </summary>
        private readonly Image OutputImage;
        private readonly object OutputImageLock;

        /// <summary>
        /// Lines left to render
        /// This is used to give tasks jobs to run
        /// Each line index represents a line of the output image
        /// </summary>
        private readonly ConcurrentBag<int> LinesLeftToRender;

        /// <summary>
        /// Scene containing all primitives to trace against
        /// </summary>
        private readonly List<PrimitiveBase> Scene;

        /// <summary>
        /// Construct a new renderer
        /// </summary>
        public Renderer()
        {
            // Retrieve ray configuration information
            MinRayLength            = ConfigurationParser.GetDouble("MIN_RAY_LENGTH");
            MaxRayLength            = ConfigurationParser.GetDouble("MAX_RAY_LENGTH");
            MaxBounces              = ConfigurationParser.GetInt("MAX_BOUNCES");
            SamplesPerPixel         = ConfigurationParser.GetInt("SAMPLES_PER_PIXEL");

            // Retrieve camera configuration information
            Exposure                = ConfigurationParser.GetDouble("EXPOSURE");
            FieldOfView             = ConfigurationParser.GetDouble("FIELD_OF_VIEW");

            // Retrieve image output configuration information
            OutputWidth             = ConfigurationParser.GetInt("IMAGE_WIDTH");
            OutputHeight            = ConfigurationParser.GetInt("IMAGE_HEIGHT");
            SaveDirectory           = ConfigurationParser.GetString("SAVE_DIRECTORY");
            FileName                = ConfigurationParser.GetString("FILE_NAME");

            OutputImage             = new Image(OutputWidth, OutputHeight);
            OutputImageLock         = new object();

            LinesLeftToRender       = new ConcurrentBag<int>();

            Scene                   = new List<PrimitiveBase>();
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
        /// Start rendering the scene on as many threads as possible
        /// </summary>
        public void Start()
        {
            // Generate a list of line indices to render
            for (int i = 0; i < OutputHeight; ++i)
            {
                LinesLeftToRender.Add(i);
            }

            // Determine the optimal number of threads to run
            int processorCount = Environment.ProcessorCount;
            Thread[] threads = new Thread[processorCount];

            Console.WriteLine("[INFO] Renderer will use " + processorCount + " logical processors to render.");

            // Spawn threads
            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(new ThreadStart(RenderTask));
                threads[i].Name = "TraceSharp_Thread_" + i;
                threads[i].Priority = ThreadPriority.Highest;
                threads[i].Start();
            }

            Console.WriteLine("[INFO] Render tasks have started, waiting...");

            // Wait for all jobs to complete
            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i].Join();
            }

            Console.WriteLine("[INFO] Render tasks have finished executing.");
        }

        /// <summary>
        /// Task that will keep grabbing a new line from the list of lines left to render
        /// The task will render the line and save it to the image
        /// Once a line is done, a new line will be fetched from the list until no lines are left
        /// </summary>
        public void RenderTask()
        {
            // One random number generator instance per thread as it is not thread safe
            Random randomNumberGenerator = new Random(Guid.NewGuid().GetHashCode());

            while (true)
            {
                // Collection is empty, no more lines left to render
                if (!LinesLeftToRender.TryTake(out int lineIndex))
                {
                    break;
                }

                // Render this line
                Color[] pixelData = ProcessLine(lineIndex, randomNumberGenerator);
                
                // Save the line data
                for (int i = 0; i < pixelData.Length; ++i)
                {
                    // Post processing steps
                    Color finalColor = ApplyPostProcessing(pixelData[i]);

                    // Index in the final image
                    int absolutePixelIndex = (lineIndex * OutputWidth) + i;

                    // Store the result
                    lock (OutputImageLock)
                    {
                        OutputImage.SetPixel(absolutePixelIndex, finalColor);
                    }
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
        /// <param name="randomNumberGenerator">Random number generator used on this thread</param>
        /// <returns>Array of pixel colors</returns>
        public Color[] ProcessLine(int lineIndex, Random randomNumberGenerator)
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
                    double subPixelJitterU = randomNumberGenerator.NextDouble() - 0.5d;
                    double subPixelJitterV = randomNumberGenerator.NextDouble() - 0.5d;

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

                    // Simulate light traveling through the scene
                    Color color = TracePixel(cameraRay, randomNumberGenerator);

                    // Average the color over the number of samples
                    // Each sample has less impact on the final image than the previous sample
                    outputColor = Color.Mix(previousFrameColor, color, 1.0d / (i + 1));
                    previousFrameColor = outputColor;
                }

                output[uInt] = outputColor;
            }

            return output;
        }

        /// <summary>
        /// Simulate a ray of light for a single pixel
        /// </summary>
        /// <param name="ray">Ray used to simulate the light for</param>
        /// <param name="randomNumberGenerator">Random number generator used on this thread</param>
        /// <returns>Color of the pixel</returns>
        public Color TracePixel(Ray ray, Random randomNumberGenerator)
        {
            Color color = Color.Black;
            Color throughput = Color.White;

            // Epsilon is used to prevent the new ray origin from starting inside the surface
            const double EPSILON = 0.01d;

            for (int i = 0; i < MaxBounces; ++i)
            {
                PrimitiveHitInfo closestHitInfo = new PrimitiveHitInfo()
                {
                    Distance = MaxRayLength
                };

                // Trace against the scene
                foreach (PrimitiveBase primitive in Scene)
                {
                    PrimitiveHitInfo hitInfo = primitive.TestRayIntersection(ray, MinRayLength, MaxRayLength);

                    // Save the result if the hit is closer to the camera than the current closest hit
                    if (hitInfo.DidHit && hitInfo.Distance < closestHitInfo.Distance)
                    {
                        closestHitInfo = hitInfo;
                    }
                }

                // Ray missed
                if (closestHitInfo.Distance == MaxRayLength)
                {
                    break;
                }

                // Update the ray's position to be on the surface of the primitive
                ray.Origin = (ray.Origin + ray.Direction * closestHitInfo.Distance) + closestHitInfo.Normal * EPSILON;

                // Calculate whether the next ray is going to be a diffuse or specular ray
                bool useSpecular = (randomNumberGenerator.NextDouble() < closestHitInfo.SurfaceMaterial.Specularness);

                // Diffuse rays use cosine weighted hemisphere samples
                // Perfect smooth specular uses reflection rays
                // Anything in between will be linearly interpolated by the roughness squared
                // Roughness does not need to be squared, but it will help with the overall perception of roughness
                Vector3 diffuseRayDirection = (closestHitInfo.Normal + Functions.RandomUnitVector(randomNumberGenerator)).Normalized;
                Vector3 specularRayDirection = Vector3.Reflect(ray.Direction, closestHitInfo.Normal);
                specularRayDirection = Vector3.Lerp(specularRayDirection, diffuseRayDirection, closestHitInfo.SurfaceMaterial.Roughness * closestHitInfo.SurfaceMaterial.Roughness);

                // Update the ray's direction (automatically normalizes in the ray.Direction property)
                ray.Direction = (useSpecular ? specularRayDirection : diffuseRayDirection);

                // Add emissive lighting
                Color emissive = closestHitInfo.SurfaceMaterial.Emissive * closestHitInfo.SurfaceMaterial.EmissiveStrength;
                color += emissive * throughput;

                // When a ray bounces off a surface, all future lighting for that ray is multiplied by the color of that surface
                throughput *= (useSpecular ? closestHitInfo.SurfaceMaterial.Specular : closestHitInfo.SurfaceMaterial.Albedo);
            }

            return color;
        }
    }
}
