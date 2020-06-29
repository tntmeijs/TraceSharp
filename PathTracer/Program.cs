using System;
using PathTracer.Configuration;
using PathTracer.Math;
using PathTracer.Primitives;
using PathTracer.Rendering;

namespace PathTracer
{
    class Program
    {
        static readonly Random rng = new Random();

        /// <summary>
        /// Some data to easily pass around scene information
        /// </summary>
        struct RenderData
        {
            public int imageWidth;
            public int imageHeight;
            public int currentLineIndex;
            public int maxBounces;
            public int samplesPerPixel;
            public double minRayLength;
            public double maxRayLength;
            public double fieldOfView;
        }

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

        /// <summary>
        /// Render a single line of the image
        /// </summary>
        /// <returns>Array of colors for the pixels on this line</returns>
        static Color[] RenderLine(RenderData data)
        {
            Color[] output = new Color[data.imageWidth];

            for (int uInt = 0; uInt < data.imageWidth; ++uInt)
            {
                // FOV to camera distance
                double cameraDistance = 1.0d / System.Math.Tan(data.fieldOfView * 0.5d * System.Math.PI / 180.0d);

                // Trace the scene for this pixel
                Color outputColor = Color.Black;
                Color previousFrameColor = Color.White;

                for (int i = 0; i < data.samplesPerPixel; ++i)
                {
                    // Sub-pixel jitter for anti-aliasing
                    double subPixelJitterU = rng.NextDouble() - 0.5d;
                    double subPixelJitterV = rng.NextDouble() - 0.5d;

                    // Normalized UV coordinates
                    double u = (uInt + subPixelJitterU) / data.imageWidth;
                    double v = (data.currentLineIndex + subPixelJitterV) / data.imageHeight;

                    // Invert the vertical range to flip the image to the correct orientation
                    v = 1.0d - v;

                    // Transform from 0 - 1 to -1 - 1
                    u = (u * 2.0d) - 1.0d;
                    v = (v * 2.0d) - 1.0d;

                    // Correct for the aspect ratio
                    double aspectRatio = (double)data.imageWidth / data.imageHeight;
                    v /= aspectRatio;

                    // Ray starts at the camera origin and goes through the imaginary pixel rectangle
                    Ray cameraRay = new Ray(Vector3.Zero, new Vector3(u, v, cameraDistance));

                    Color color = CalculatePixelColor(cameraRay, data.minRayLength, data.maxRayLength, data.maxBounces);

                    // Average the color over the number of samples
                    // Each sample has less impact on the final image than the previous sample
                    outputColor = Color.Mix(previousFrameColor, color, 1.0d / (i + 1));
                    previousFrameColor = outputColor;
                }

                output[uInt] = outputColor;
            }

            return output;
        }

        static void Main(string[] args)
        {
            // Retrieve ray configuration information
            double minRayLength     = ConfigurationParser.GetDouble("MIN_RAY_LENGTH");
            double maxRayLength     = ConfigurationParser.GetDouble("MAX_RAY_LENGTH");
            int maxBounces          = ConfigurationParser.GetInt("MAX_BOUNCES");
            int samplesPerPixel     = ConfigurationParser.GetInt("SAMPLES_PER_PIXEL");

            // Retrieve camera configuration information
            double exposure         = ConfigurationParser.GetDouble("EXPOSURE");
            double fieldOfView      = ConfigurationParser.GetDouble("FIELD_OF_VIEW");

            // Retrieve image output configuration information
            int outputWidth         = ConfigurationParser.GetInt("IMAGE_WIDTH");
            int outputHeight        = ConfigurationParser.GetInt("IMAGE_HEIGHT");
            string saveDirectory    = ConfigurationParser.GetString("SAVE_DIRECTORY");
            string fileName         = ConfigurationParser.GetString("FILE_NAME");

            Image outputImage = new Image(outputWidth, outputHeight);

            // Render information needed when tracing the scene
            RenderData data = new RenderData();
            data.imageWidth = outputWidth;
            data.imageHeight = outputHeight;
            data.maxBounces = maxBounces;
            data.samplesPerPixel = samplesPerPixel;
            data.fieldOfView = fieldOfView;
            data.minRayLength = minRayLength;
            data.maxRayLength = maxRayLength;
            data.currentLineIndex = 0;

            // Calculate a color for every pixel in the image
            int pixelIndex = 0;
            for (int vInt = 0; vInt < outputHeight; ++vInt)
            {
                data.currentLineIndex = vInt;

                Color[] pixelData = RenderLine(data);

                // Save the line data in the final image
                foreach (Color pixel in pixelData)
                {
                    Color finalColor = pixel;

                    // Post processing steps
                    finalColor *= exposure;
                    finalColor = Color.ToneMapACES(finalColor);
                    finalColor = Color.GammaCorrection(finalColor);

                    outputImage.SetPixel(pixelIndex++, finalColor);
                }
            }

            // Store the result on disk to allow the user to see the results of the path tracer
            if (!outputImage.SaveToPpm(saveDirectory, fileName))
            {
                Console.WriteLine("[ERROR] Failed to write ppm file to disk.");
            }

            Console.WriteLine("\n\nPress <Enter> to continue.");
            Console.Read();
        }
    }
}
