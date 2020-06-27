using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Math;

namespace UnitTests.Math
{
    [TestClass]
    public class TestRay
    {
        [TestMethod]
        public void TestRayEmptyConstructor()
        {
            Ray ray = new Ray();

            Assert.IsTrue(ray.Origin.Equals(Vector3.Zero),                      "Ray origin incorrect.");
            Assert.IsTrue(ray.Direction.Equals(new Vector3(0.0d, 0.0d, 1.0d)),  "Ray direction incorrect.");
        }

        [TestMethod]
        public void TestRayConstructor()
        {
            Ray ray = new Ray(new Vector3(1.0d, 1.0d, 1.0d), new Vector3(0.0d, 0.0d, 4.0d));

            Assert.IsTrue(ray.Origin.Equals(new Vector3(1.0d, 1.0d, 1.0d)),     "Ray origin incorrect.");
            Assert.IsTrue(ray.Direction.Equals(new Vector3(0.0d, 0.0d, 1.0d)),  "Ray direction incorrect.");
        }
    }
}
