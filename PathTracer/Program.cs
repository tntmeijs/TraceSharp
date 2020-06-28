using System;

using PathTracer.Math;
using PathTracer.Primitives;
using PathTracer.Rendering;

namespace PathTracer
{
    class Program
    {
        static readonly Random rng = new Random();

        /// <summary>
        /// Cosine weighted random unit vector generation
        /// </summary>
        /// <returns></returns>
        static Vector3 RandomUnitVector()
        {
            double z = rng.NextDouble() * 2.0d - 1.0d;
            double a = rng.NextDouble() * System.Math.PI * 2.0d;
            double r = System.Math.Sqrt(1.0d - z * z);
            double x = r * System.Math.Cos(a);
            double y = r * System.Math.Sin(a);

            return new Vector3(x, y, z);
        }

        static void TraceScene(Ray ray, double minHitDistance, double maxHitDistance, ref PrimitiveHitInfo hitInfo)
        {
            // Walls
            Material backWallMat        = new Material(new Color(0.7d, 0.7d, 0.7d), new Color(0.0d, 0.0d, 0.0d));
            Material floorMat           = new Material(new Color(0.7d, 0.7d, 0.7d), new Color(0.0d, 0.0d, 0.0d));
            Material ceilingMat         = new Material(new Color(0.7d, 0.7d, 0.7d), new Color(0.0d, 0.0d, 0.0d));
            Material leftWallMat        = new Material(new Color(0.7d, 0.1d, 0.1d), new Color(0.0d, 0.0d, 0.0d));
            Material rightWallMat       = new Material(new Color(0.1d, 0.7d, 0.1d), new Color(0.0d, 0.0d, 0.0d));
            
            // Light
            Material lightSourceMat     = new Material(new Color(0.0d, 0.0d, 0.0d), new Color(1.0d, 0.9d, 0.7d), 20.0d);

            // Objects
            Material yellowSphereMat    = new Material(new Color(0.9d, 1.0d, 0.3d), new Color(0.0d, 0.0d, 0.0d));
            Material pinkSphereMat      = new Material(new Color(1.0d, 0.2d, 1.0d), new Color(0.0d, 0.0d, 0.0d));
            Material tealSphereMat      = new Material(new Color(0.0d, 0.8d, 0.8d), new Color(0.0d, 0.0d, 0.0d));

            // Object that somewhat resembles the walls of a Cornell Box
            QuadPrimitive backWall = new QuadPrimitive(
                new Vector3(-12.6d, -12.6d, 35.0d),
                new Vector3( 12.6d, -12.6d, 35.0d),
                new Vector3( 12.6d,  12.6d, 35.0d),
                new Vector3(-12.6d,  12.6d, 35.0d),
                backWallMat);

            QuadPrimitive floor = new QuadPrimitive(
                new Vector3(-12.6d, -12.45d, 35.0d),
                new Vector3( 12.6d, -12.45d, 35.0d),
                new Vector3( 12.6d, -12.45d, 25.0d),
                new Vector3(-12.6d, -12.45d, 25.0d),
                floorMat);

            QuadPrimitive ceiling = new QuadPrimitive(
                new Vector3(-12.6d,  12.5d, 35.0d),
                new Vector3( 12.6d,  12.5d, 35.0d),
                new Vector3( 12.6d,  12.5d, 25.0d),
                new Vector3(-12.6d,  12.5d, 25.0d),
                ceilingMat);

            QuadPrimitive leftWall = new QuadPrimitive(
                new Vector3(-12.5d, -12.6d, 35.0d),
                new Vector3(-12.5d, -12.6d, 25.0d),
                new Vector3(-12.5d,  12.6d, 25.0d),
                new Vector3(-12.5d,  12.6d, 35.0d),
                leftWallMat);

            QuadPrimitive rightWall = new QuadPrimitive(
                new Vector3( 12.5d, -12.6d, 35.0d),
                new Vector3( 12.5d, -12.6d, 25.0d),
                new Vector3( 12.5d,  12.6d, 25.0d),
                new Vector3( 12.5d,  12.6d, 35.0d),
                rightWallMat);

            // Light source
            QuadPrimitive lightSource = new QuadPrimitive(
                new Vector3(-5.0d,  12.4d, 32.5d),
                new Vector3( 5.0d,  12.4d, 32.5d),
                new Vector3( 5.0d,  12.4d, 27.5d),
                new Vector3(-5.0d,  12.4d, 27.5d),
                lightSourceMat);

            // Objects
            SpherePrimitive leftSphere      = new SpherePrimitive(new Vector3(-9.0d, -9.5d, 30.0d), 3.0d, yellowSphereMat);
            SpherePrimitive centerSphere    = new SpherePrimitive(new Vector3( 0.0d, -9.5d, 30.0d), 3.0d, pinkSphereMat);
            SpherePrimitive rightSphere     = new SpherePrimitive(new Vector3( 9.0d, -9.5d, 30.0d), 3.0d, tealSphereMat);

            if (backWall.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = backWall.Material.Albedo;
                hitInfo.Emissive = backWall.Material.Emissive * backWall.Material.EmissiveStrength;
            }

            if (floor.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = floor.Material.Albedo;
                hitInfo.Emissive = floor.Material.Emissive * floor.Material.EmissiveStrength;
            }

            if (ceiling.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = ceiling.Material.Albedo;
                hitInfo.Emissive = ceiling.Material.Emissive * ceiling.Material.EmissiveStrength;
            }

            if (leftWall.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = leftWall.Material.Albedo;
                hitInfo.Emissive = leftWall.Material.Emissive * leftWall.Material.EmissiveStrength;
            }

            if (rightWall.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = rightWall.Material.Albedo;
                hitInfo.Emissive = rightWall.Material.Emissive * rightWall.Material.EmissiveStrength;
            }

            if (lightSource.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = lightSource.Material.Albedo;
                hitInfo.Emissive = lightSource.Material.Emissive * lightSource.Material.EmissiveStrength;
            }

            if (leftSphere.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = leftSphere.Material.Albedo;
                hitInfo.Emissive = leftSphere.Material.Emissive * leftSphere.Material.EmissiveStrength;
            }

            if (centerSphere.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = centerSphere.Material.Albedo;
                hitInfo.Emissive = centerSphere.Material.Emissive * centerSphere.Material.EmissiveStrength;
            }

            if (rightSphere.TestRayIntersection(ray, minHitDistance, maxHitDistance, ref hitInfo))
            {
                hitInfo.Albedo = rightSphere.Material.Albedo;
                hitInfo.Emissive = rightSphere.Material.Emissive * rightSphere.Material.EmissiveStrength;
            }
        }

        static Color CalculatePixelColor(Ray ray, double minLength, double maxLength, int maxBounces)
        {
            Color color = Color.Black;
            Color throughput = Color.White;

            // Epsilon is used to prevent the new ray origin from starting inside the surface
            const double EPSILON = 0.01d;

            Vector3 rayPos = ray.Origin;
            Vector3 rayDir = ray.Direction;

            for (int i = 0; i < maxBounces; ++i)
            {
                PrimitiveHitInfo hitInfo = new PrimitiveHitInfo();
                hitInfo.Distance = maxLength;

                TraceScene(new Ray(rayPos, rayDir), minLength, maxBounces, ref hitInfo);

                // Ray missed
                if (hitInfo.Distance == maxLength)
                {
                    break;
                }

                // Construct a new ray
                rayPos = (rayPos + rayDir * hitInfo.Distance) + hitInfo.Normal * EPSILON;
                rayDir = hitInfo.Normal + RandomUnitVector();

                // Add emissive lighting
                color += hitInfo.Emissive * throughput;

                // When a ray bounces off a surface, all future lighting for that ray is multiplied by the color of that surface
                throughput *= hitInfo.Albedo;
            }

            return color;
        }

        static void Main(string[] args)
        {
            // Minimum distance a ray must travel before an intersection is considered
            // This prevents the ray from intersecting a surface it just bounced from
            const double MINIMUM_RAY_LENGTH = 0.01d;

            // Maximum length of a ray before it is considered a miss
            const double MAXIMUM_RAY_LENGTH = 10000.0d;

            // Camera field of view in degrees
            const double CAMERA_FOV = 90.0d;

            // Maximum number of bounces per ray
            const int MAX_BOUNCES = 4;

            // Samples per pixel
            const int SAMPLES_PER_PIXEL = 8;

            // The path tracer will create images with a resolution of 1280x720
            const int OUTPUT_WIDTH  = 1280;
            const int OUTPUT_HEIGHT =  720;

            // Output images will be saved here
            const string OUTPUT_PATH = "./";
            const string OUTPUT_NAME = "TraceSharp_output";

            Image outputImage = new Image(OUTPUT_WIDTH, OUTPUT_HEIGHT);

            // Keep track of total render time
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            int previousPercentage = 0;

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

                    // Correct for the aspect ratio
                    double aspectRatio = (double)OUTPUT_WIDTH / OUTPUT_HEIGHT;
                    v /= aspectRatio;

                    // FOV to camera distance
                    double cameraDistance = 1.0d / System.Math.Tan(CAMERA_FOV * 0.5d * System.Math.PI / 180.0d);

                    // Ray starts at the camera origin and goes through the imaginary pixel rectangle
                    Ray cameraRay = new Ray(Vector3.Zero, new Vector3(u, v, cameraDistance));

                    // Trace the scene for this pixel
                    Color outputColor = Color.Black;
                    Color previousFrameColor = Color.White;

                    for (int i = 0; i < SAMPLES_PER_PIXEL; ++i)
                    {
                        Color color = CalculatePixelColor(cameraRay, MINIMUM_RAY_LENGTH, MAXIMUM_RAY_LENGTH, MAX_BOUNCES);
                        
                        // Average the color over the number of samples
                        // Each sample has less impact on the final image than the previous sample
                        outputColor = Color.Mix(previousFrameColor, color, 1.0d / (i + 1));
                        previousFrameColor = outputColor;
                    }

                    // Save the data to disk
                    outputImage.SetPixel(pixelIndex++, outputColor);

                    // Track render progress
                    int percentageDone = (int)System.Math.Ceiling(pixelIndex / (double)(OUTPUT_WIDTH * OUTPUT_HEIGHT) * 100.0d);
                    if (percentageDone % 5 == 0 && percentageDone > previousPercentage)
                    {
                        previousPercentage = percentageDone;

                        long timeElapsedSeconds = (long)stopWatch.Elapsed.TotalSeconds;
                        string suffix = timeElapsedSeconds == 1 ? "second" : "seconds";

                        Console.WriteLine("Progress: " + percentageDone + "%\t-\ttook " + timeElapsedSeconds + "\t" + suffix + ".");
                    }
                }
            }

            // Store the result on disk to allow the user to see the results of the path tracer
            if (!outputImage.SaveToPpm(OUTPUT_PATH, OUTPUT_NAME))
            {
                Console.WriteLine("[ERROR] Failed to write ppm file to disk.");
            }

            stopWatch.Stop();
            Console.WriteLine("\n\nPress <Enter> to continue.");
            Console.Read();
        }
    }
}
