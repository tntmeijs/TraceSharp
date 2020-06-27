﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Math;
using PathTracer.Primitives;

namespace UnitTests.Primitives
{
    [TestClass]
    public class TestSpherePrimitive
    {
        [TestMethod]
        public void TestEmptyConstructorSpherePrimitive()
        {
            SpherePrimitive sphere = new SpherePrimitive();

            Assert.IsTrue(sphere.Center.Equals(Vector3.Zero),   "Sphere center is not at (0, 0, 0).");
            Assert.IsTrue(sphere.Radius == 1.0d,                "Sphere radius does not equal 1.");
        }

        [TestMethod]
        public void TestConstructorSpherePrimitive()
        {
            SpherePrimitive sphere = new SpherePrimitive(new Vector3(1.0d, 0.0d, 5.0d), 0.5d);

            Assert.IsTrue(sphere.Center.Equals(new Vector3(1.0d, 0.0d, 5.0d)),  "Sphere center is not at (1, 0, 5).");
            Assert.IsTrue(sphere.Radius == 0.5d,                                "Sphere radius does not equal 0.5.");
        }

        [TestMethod]
        public void TestRayIntersectInsideSpherePrimitive()
        {
            SpherePrimitive sphere = new SpherePrimitive();
            PrimitiveHitInfo hitInfo;
            Ray ray = new Ray();

            // Test if the ray hits the sphere when the ray starts inside the sphere
            bool hit = sphere.TestRayIntersection(ray, 0.1d, 10000.0d, out hitInfo);

            Assert.IsTrue(hit,                                  "Ray starts inside the sphere, this should be a hit.");
            Assert.IsTrue(hitInfo.Distance == sphere.Radius,    "Hit distance should equal the sphere's radius.");
        }

        [TestMethod]
        public void TestRayIntersectInFrontSpherePrimitive()
        {
            SpherePrimitive sphere = new SpherePrimitive();
            PrimitiveHitInfo hitInfo;
            Ray ray = new Ray(new Vector3(0.0d, 0.0d, -5.0d), Vector3.Forward);

            // Test if the ray hits the sphere when the ray stars in behind of the sphere
            bool hit = sphere.TestRayIntersection(ray, 0.1d, 10000.0d, out hitInfo);

            Assert.IsTrue(hit,                      "Ray starts in front of the sphere, this should be a hit.");
            Assert.IsTrue(hitInfo.Distance == 4.0d, "Hit distance incorrect.");
        }

        [TestMethod]
        public void TestRayIntersectBehindSpherePrimitive()
        {
            SpherePrimitive sphere = new SpherePrimitive();
            PrimitiveHitInfo hitInfo;
            Ray ray = new Ray(new Vector3(0.0d, 0.0d, 5.0d), Vector3.Forward);

            // Test if the ray hits the sphere when the ray stars in behind of the sphere
            bool hit = sphere.TestRayIntersection(ray, 0.1d, 10000.0d, out hitInfo);

            Assert.IsFalse(hit, "Ray starts in behind of the sphere, this should never be a hit.");
        }

        [TestMethod]
        public void TestRayIntersectOutOfBoundsSpherePrimitive()
        {
            const double NEAR   = 0.1d;
            const double FAR    = 10000.0d;

            SpherePrimitive sphere = new SpherePrimitive();
            PrimitiveHitInfo hitInfo;

            // Far out of bounds (miss)
            Ray ray = new Ray(new Vector3(0.0d, 0.0d, -15000.0d), Vector3.Forward);
            bool farHit = sphere.TestRayIntersection(ray, NEAR, FAR, out hitInfo);

            // Near out of bounds (too close)
            ray = new Ray(new Vector3(0.0d, 0.0d, -0.05d), Vector3.Forward);
            bool nearHit = sphere.TestRayIntersection(ray, NEAR, FAR, out hitInfo);

            Assert.IsFalse(farHit,  "Ray length too long, this should miss the sphere.");
            Assert.IsFalse(nearHit, "Ray length too short, this should miss the sphere.");
        }
    }
}
