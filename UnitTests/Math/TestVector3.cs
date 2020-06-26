using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Math;

namespace UnitTests.Math
{
    [TestClass]
    public class TestVector3
    {
        [TestMethod]
        public void TestInitializeConstructorVector3()
        {
            Vector3 A = new Vector3(1.0d, 2.0d, 3.0d);

            Assert.IsTrue(A.X == 1.0d, "Vector X component did not initialize correctly.");
            Assert.IsTrue(A.Y == 2.0d, "Vector Y component did not initialize correctly.");
            Assert.IsTrue(A.Z == 3.0d, "Vector Z component did not initialize correctly.");
        }

        [TestMethod]
        public void TestInitializeZeroVector3()
        {
            Vector3 A = Vector3.Zero;

            Assert.IsTrue(A.X == 0.0d, "Vector X component did not initialize correctly.");
            Assert.IsTrue(A.Y == 0.0d, "Vector Y component did not initialize correctly.");
            Assert.IsTrue(A.Z == 0.0d, "Vector Z component did not initialize correctly.");
        }

        [TestMethod]
        public void TestOperatorInvertVector3()
        {
            Vector3 A = new Vector3(1.0d, -2.0d, 0.0d);
            Vector3 inverted = -A;

            Assert.IsTrue(inverted.X == -1.0d, "Vector X component did not invert correctly.");
            Assert.IsTrue(inverted.Y ==  2.0d, "Vector Y component did not invert correctly.");
            Assert.IsTrue(inverted.Z ==  0.0d, "Vector Z component did not invert correctly.");
        }

        [TestMethod]
        public void TestOperatorNothingVector3()
        {
            // Writing +Vector3 will do nothing to the values
            Vector3 A = new Vector3(1.0d, 2.0d, 3.0d);
            Vector3 nothing = +A;

            Assert.IsTrue(nothing.X == 1.0d, "Vector X component changed.");
            Assert.IsTrue(nothing.Y == 2.0d, "Vector Y component changed.");
            Assert.IsTrue(nothing.Z == 3.0d, "Vector Z component changed.");
        }

        [TestMethod]
        public void TestOperatorSubtractVector3()
        {
            Vector3 lhs = new Vector3(1.0f, 1.0f,  1.0f);
            Vector3 rhs = new Vector3(1.0f, 2.0f, -4.0f);
            Vector3 sub = lhs - rhs;

            Assert.IsTrue(sub.X ==  0.0d, "Vector X component did not subtract correctly.");
            Assert.IsTrue(sub.Y == -1.0d, "Vector Y component did not subtract correctly.");
            Assert.IsTrue(sub.Z ==  5.0d, "Vector Z component did not subtract correctly.");
        }

        [TestMethod]
        public void TestOperatorAddVector3()
        {
            Vector3 lhs = new Vector3(1.0f, 1.0f,  1.0f);
            Vector3 rhs = new Vector3(1.0f, 2.0f, -4.0f);
            Vector3 add = lhs + rhs;

            Assert.IsTrue(add.X ==  2.0d, "Vector X component did not add correctly.");
            Assert.IsTrue(add.Y ==  3.0d, "Vector Y component did not add correctly.");
            Assert.IsTrue(add.Z == -3.0d, "Vector Z component did not add correctly.");
        }

        [TestMethod]
        public void TestOperatorMultiplyVector3()
        {
            Vector3 lhs = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 rhs = new Vector3(1.0f, 2.0f, -4.0f);
            Vector3 mtp = lhs * rhs;

            Assert.IsTrue(mtp.X ==  1.0d, "Vector X component did not multiply correctly.");
            Assert.IsTrue(mtp.Y ==  2.0d, "Vector Y component did not multiply correctly.");
            Assert.IsTrue(mtp.Z == -4.0d, "Vector Z component did not multiply correctly.");
        }

        [TestMethod]
        public void TestDotVector3()
        {
            Vector3 A = new Vector3(1.0f, 3.0f, -5.0f);
            Vector3 B = new Vector3(4.0f, -2.0f, -1.0f);
            double dot = Vector3.Dot(A, B);

            Assert.IsTrue(dot == 3.0d, "Incorrect dot product result.");
        }

        [TestMethod]
        public void TestCrossVector3()
        {
            Vector3 A = new Vector3(2.0f, 3.0f, 4.0f);
            Vector3 B = new Vector3(5.0f, 6.0f, 7.0f);
            Vector3 cross = Vector3.Cross(A, B);

            Assert.IsTrue(cross.X == -3.0d && cross.Y == 6.0d && cross.Z == -3.0d, "Incorrect cross product result.");
        }

        [TestMethod]
        public void TestMagnitudeVector3()
        {
            Vector3 A = new Vector3(6.0d, 8.0d, 0.0d);

            Assert.IsTrue(A.Magnitude == 10.0d, "Incorrect vector magnitude.");
        }
    }
}
