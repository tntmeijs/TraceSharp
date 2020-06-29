using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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
        private readonly object OutputImageLock;

        private readonly Random RandomNumberGenerator;

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

            RandomNumberGenerator   = new Random();

            LinesLeftToRender       = new ConcurrentBag<int>();

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
        /// Start rendering the scene on as many threads as possible
        /// </summary>
        public void Start()
        {
            // Generate a list of line indices to render
            for (int i = 0; i < OutputHeight; ++i)
            {
                LinesLeftToRender.Add(i);
            }

            // Spawn tasks
            int processorCount = 1; //#DEBUG: Multithreading does not work yet!
            Task[] tasks = new Task[processorCount];

            Console.WriteLine("[INFO] Renderer will use " + processorCount + " logical processors to render.");

            // Start rendering on all available logical processors
            for (int i = 0; i < tasks.Length; ++i)
            {
                tasks[i] = Task.Factory.StartNew(RenderTask);
            }

            Console.WriteLine("[INFO] Render tasks have started, waiting...");

            // Wait for the job to complete
            Task.WaitAll(tasks);

            Console.WriteLine("[INFO] Render tasks have finished executing.");
        }

        /// <summary>
        /// Task that will keep grabbing a new line from the list of lines left to render
        /// The task will render the line and save it to the image
        /// Once a line is done, a new line will be fetched from the list until no lines are left
        /// </summary>
        public void RenderTask()
        {
            while (true)
            {
                // Collection is empty, no more lines left to render
                if (!LinesLeftToRender.TryTake(out int lineIndex))
                {
                    break;
                }

                // Render this line
                Color[] pixelData = ProcessLine(lineIndex);

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

        public object obj = new object();

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
                PrimitiveHitInfo closestHitInfo = new PrimitiveHitInfo()
                {
                    Distance = MaxRayLength
                };

                // Trace against the scene
                foreach (PrimitiveBase primitive in Scene)
                {
                    PrimitiveHitInfo hitInfo = primitive.TestRayIntersection(new Ray(rayPos, rayDir), MinRayLength, MaxRayLength);

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

                // Construct a new ray
                rayPos = (rayPos + rayDir * closestHitInfo.Distance) + closestHitInfo.Normal * EPSILON;
                rayDir = (closestHitInfo.Normal + Functions.RandomUnitVector(RandomNumberGenerator)).Normalized;

                // Add emissive lighting
                color += closestHitInfo.Emissive * throughput;

                // When a ray bounces off a surface, all future lighting for that ray is multiplied by the color of that surface
                throughput *= closestHitInfo.Albedo;
            }

            return color;
        }
    }
}
