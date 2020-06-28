using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Math;

namespace UnitTests.Math
{
    [TestClass]
    public class TestFunctions
    {
        [TestMethod]
        public void TestClampFunctions()
        {
            Assert.IsTrue(Functions.Clamp(7.0d, 0.0d, 5.0d) == 5.0d, "Clamping to upper bound failed.");
            Assert.IsTrue(Functions.Clamp(-3.0d, 0.0d, 5.0d) == 0.0d, "Clamping to lower bound failed.");
            Assert.IsTrue(Functions.Clamp(2.5d, 0.0d, 5.0d) == 2.5d, "Value should remain the same.");

            Assert.IsTrue(Functions.Clamp01(7.0d) == 1.0d, "Clamping to upper bound failed.");
            Assert.IsTrue(Functions.Clamp01(-3.0d) == 0.0d, "Clamping to lower bound failed.");
            Assert.IsTrue(Functions.Clamp(0.5d, 0.0d, 5.0d) == 0.5d, "Value should remain the same.");
        }
    }
}
