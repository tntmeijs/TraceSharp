using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Math;
using PathTracer.Primitives;
using PathTracer.Rendering;

namespace UnitTests.Primitives
{
    [TestClass]
    public class TestQuadPrimitive
    {
        [TestMethod]
        public void TestEmptyConstructorQuadPrimitive()
        {
            QuadPrimitive quad = new QuadPrimitive(new Material(Color.Black, Color.Black));

            Assert.IsTrue(quad.BottomLeft.Equals(new Vector3(-0.5d, -0.5d, 0.0d)),  "Quad bottom left corner incorrect.");
            Assert.IsTrue(quad.BottomRight.Equals(new Vector3(0.5d, -0.5d, 0.0d)),  "Quad bottom right corner incorrect.");
            Assert.IsTrue(quad.TopRight.Equals(new Vector3(0.5d, 0.5d, 0.0d)),      "Quad top right corner incorrect.");
            Assert.IsTrue(quad.TopLeft.Equals(new Vector3(-0.5d, 0.5d, 0.0d)),      "Quad top left corner incorrect.");
        }

        [TestMethod]
        public void TestConstructorQuadPrimitive()
        {
            QuadPrimitive quad = new QuadPrimitive(
                new Vector3(-15.0d, -15.0d, 22.0d),
                new Vector3(15.0d, -15.0d, 22.0d),
                new Vector3(15.0d, 15.0d, 22.0d),
                new Vector3(-15.0d, 15.0d, 22.0d),
                new Material(Color.Black, Color.Black));

            Assert.IsTrue(quad.BottomLeft.Equals(new Vector3(-15.0d, -15.0d, 22.0d)),   "Quad bottom left corner incorrect.");
            Assert.IsTrue(quad.BottomRight.Equals(new Vector3(15.0d, -15.0d, 22.0d)),   "Quad bottom right corner incorrect.");
            Assert.IsTrue(quad.TopRight.Equals(new Vector3(15.0d, 15.0d, 22.0d)),       "Quad top right corner incorrect.");
            Assert.IsTrue(quad.TopLeft.Equals(new Vector3(-15.0d, 15.0d, 22.0d)),       "Quad top left corner incorrect.");
        }

        [TestMethod]
        public void TestRayIntersectInsideQuadPrimitive()
        {
            QuadPrimitive quad = new QuadPrimitive(new Material(Color.Black, Color.Black));
            PrimitiveHitInfo hitInfo = new PrimitiveHitInfo();
            Ray ray = new Ray();

            // Test if the ray hits the quad when the ray starts inside the quad
            bool hit = quad.TestRayIntersection(ray, 0.1d, 10000.0d, ref hitInfo);

            Assert.IsFalse(hit, "Ray starts inside the quad, this should not be a hit.");
        }

        [TestMethod]
        public void TestRayIntersectBehindQuadPrimitive()
        {
            QuadPrimitive quad = new QuadPrimitive(new Material(Color.Black, Color.Black));
            PrimitiveHitInfo hitInfo = new PrimitiveHitInfo();
            Ray ray = new Ray(new Vector3(0.0d, 0.0d, 5.0d), Vector3.Forward);

            // Test if the ray hits the quad when the ray stars in behind of the quad
            bool hit = quad.TestRayIntersection(ray, 0.1d, 10000.0d, ref hitInfo);

            Assert.IsFalse(hit, "Ray starts in behind of the quad, this should never be a hit.");
        }

        [TestMethod]
        public void TestRayIntersectOutOfBoundsQuadPrimitive()
        {
            const double NEAR   = 0.1d;
            const double FAR    = 10000.0d;

            QuadPrimitive quad = new QuadPrimitive(new Material(Color.Black, Color.Black));
            PrimitiveHitInfo hitInfo = new PrimitiveHitInfo();

            // Far ref of bounds (miss)
            Ray ray = new Ray(new Vector3(0.0d, 0.0d, -15000.0d), Vector3.Forward);
            bool farHit = quad.TestRayIntersection(ray, NEAR, FAR, ref hitInfo);

            // Near ref of bounds (too close)
            ray = new Ray(new Vector3(0.0d, 0.0d, -0.05d), Vector3.Forward);
            bool nearHit = quad.TestRayIntersection(ray, NEAR, FAR, ref hitInfo);

            Assert.IsFalse(farHit,  "Ray length too long, this should miss the quad.");
            Assert.IsFalse(nearHit, "Ray length too short, this should miss the quad.");
        }
    }
}
