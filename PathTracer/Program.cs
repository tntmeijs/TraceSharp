using System;
using PathTracer.Math;
using PathTracer.Primitives;
using PathTracer.Rendering;

namespace PathTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            Renderer renderer = new Renderer();

            // Create all materials used in this scene
            #region MATERIALS
            // Walls
            Material backWallMat = new Material(new Color(0.7d, 0.7d, 0.7d), new Color(0.0d, 0.0d, 0.0d));
            Material floorMat = new Material(new Color(0.7d, 0.7d, 0.7d), new Color(0.0d, 0.0d, 0.0d));
            Material ceilingMat = new Material(new Color(0.7d, 0.7d, 0.7d), new Color(0.0d, 0.0d, 0.0d));
            Material leftWallMat = new Material(new Color(0.7d, 0.1d, 0.1d), new Color(0.0d, 0.0d, 0.0d));
            Material rightWallMat = new Material(new Color(0.1d, 0.7d, 0.1d), new Color(0.0d, 0.0d, 0.0d));

            // Light
            Material lightSourceMat = new Material(new Color(0.0d, 0.0d, 0.0d), new Color(1.0d, 0.9d, 0.7d), 20.0d);

            // Objects
            Material yellowSphereMat = new Material(new Color(0.9d, 1.0d, 0.3d), new Color(0.0d, 0.0d, 0.0d));
            Material pinkSphereMat = new Material(new Color(1.0d, 0.2d, 1.0d), new Color(0.0d, 0.0d, 0.0d));
            Material tealSphereMat = new Material(new Color(0.0d, 0.8d, 0.8d), new Color(0.0d, 0.0d, 0.0d));
            #endregion

            // Construct a Cornell box
            #region CORNELL_BOX
            QuadPrimitive backWall = new QuadPrimitive(
                new Vector3(-12.6d, -12.6d, 35.0d),
                new Vector3(12.6d, -12.6d, 35.0d),
                new Vector3(12.6d, 12.6d, 35.0d),
                new Vector3(-12.6d, 12.6d, 35.0d),
                backWallMat);

            QuadPrimitive floor = new QuadPrimitive(
                new Vector3(-12.6d, -12.45d, 35.0d),
                new Vector3(12.6d, -12.45d, 35.0d),
                new Vector3(12.6d, -12.45d, 25.0d),
                new Vector3(-12.6d, -12.45d, 25.0d),
                floorMat);

            QuadPrimitive ceiling = new QuadPrimitive(
                new Vector3(-12.6d, 12.5d, 35.0d),
                new Vector3(12.6d, 12.5d, 35.0d),
                new Vector3(12.6d, 12.5d, 25.0d),
                new Vector3(-12.6d, 12.5d, 25.0d),
                ceilingMat);

            QuadPrimitive leftWall = new QuadPrimitive(
                new Vector3(-12.5d, -12.6d, 35.0d),
                new Vector3(-12.5d, -12.6d, 25.0d),
                new Vector3(-12.5d, 12.6d, 25.0d),
                new Vector3(-12.5d, 12.6d, 35.0d),
                leftWallMat);

            QuadPrimitive rightWall = new QuadPrimitive(
                new Vector3(12.5d, -12.6d, 35.0d),
                new Vector3(12.5d, -12.6d, 25.0d),
                new Vector3(12.5d, 12.6d, 25.0d),
                new Vector3(12.5d, 12.6d, 35.0d),
                rightWallMat);
            #endregion

            // Add light sources
            #region LIGHTS
            QuadPrimitive lightSource = new QuadPrimitive(
                new Vector3(-5.0d, 12.4d, 32.5d),
                new Vector3(5.0d, 12.4d, 32.5d),
                new Vector3(5.0d, 12.4d, 27.5d),
                new Vector3(-5.0d, 12.4d, 27.5d),
                lightSourceMat);
            #endregion

            // Spheres inside the Cornell Box
            #region BOX_CONTENTS
            SpherePrimitive leftSphere = new SpherePrimitive(new Vector3(-9.0d, -9.5d, 30.0d), 3.0d, yellowSphereMat);
            SpherePrimitive centerSphere = new SpherePrimitive(new Vector3(0.0d, -9.5d, 30.0d), 3.0d, pinkSphereMat);
            SpherePrimitive rightSphere = new SpherePrimitive(new Vector3(9.0d, -9.5d, 30.0d), 3.0d, tealSphereMat);
            #endregion

            // Add all primitives to the scene
            #region SCENE
            // Add Cornell Box primitives to the scene
            renderer.AddPrimitiveToScene(backWall);
            renderer.AddPrimitiveToScene(floor);
            renderer.AddPrimitiveToScene(ceiling);
            renderer.AddPrimitiveToScene(leftWall);
            renderer.AddPrimitiveToScene(rightWall);

            // Add light sources to the scene
            renderer.AddPrimitiveToScene(lightSource);

            // Add the primitives inside the Cornell Box to the scene
            renderer.AddPrimitiveToScene(leftSphere);
            renderer.AddPrimitiveToScene(centerSphere);
            renderer.AddPrimitiveToScene(rightSphere);
            #endregion

            // Render the scene into memory
            renderer.Start();

            // Dump the memory to disk
            renderer.SaveToDisk();

            // Wait for the user to close the console window
            Console.WriteLine("\n\nPress <Enter> to continue.");
            Console.Read();
        }
    }
}
