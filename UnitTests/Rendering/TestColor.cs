using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Math;
using PathTracer.Rendering;

namespace UnitTests.Rendering
{
    [TestClass]
    public class TestColor
    {
        [TestMethod]
        public void TestInitializeEmptyConstructorColor()
        {
            Color color = new Color();

            Assert.IsTrue(color.R == 0.0d, "Red component did not initialize correctly.");
            Assert.IsTrue(color.G == 0.0d, "Red component did not initialize correctly.");
            Assert.IsTrue(color.B == 0.0d, "Red component did not initialize correctly.");
        }

        [TestMethod]
        public void TestInitializeVector3ConstructorColor()
        {
            Color color = new Color(new Vector3(1.0d, 1.0d, 1.0d));

            Assert.IsTrue(color.R == 1.0d, "Red component did not initialize correctly.");
            Assert.IsTrue(color.G == 1.0d, "Red component did not initialize correctly.");
            Assert.IsTrue(color.B == 1.0d, "Red component did not initialize correctly.");
        }

        [TestMethod]
        public void TestInitializeComponentsConstructorColor()
        {
            Color color = new Color(1.0d, 1.0d, 1.0d);

            Assert.IsTrue(color.R == 1.0d, "Red component did not initialize correctly.");
            Assert.IsTrue(color.G == 1.0d, "Red component did not initialize correctly.");
            Assert.IsTrue(color.B == 1.0d, "Red component did not initialize correctly.");
        }

        [TestMethod]
        public void TestRedColor()
        {
            Color color = Color.Red;

            Assert.IsTrue(color.R == 1.0d && color.G == 0.0d && color.B == 0.0d, "Color is not red.");
        }

        [TestMethod]
        public void TestGreenColor()
        {
            Color color = Color.Green;

            Assert.IsTrue(color.R == 0.0d && color.G == 1.0d && color.B == 0.0d, "Color is not green.");
        }

        [TestMethod]
        public void TestBlueColor()
        {
            Color color = Color.Blue;

            Assert.IsTrue(color.R == 0.0d && color.G == 0.0d && color.B == 1.0d, "Color is not blue.");
        }

        [TestMethod]
        public void TestPurpleColor()
        {
            Color color = Color.Purple;

            Assert.IsTrue(color.R == 1.0d && color.G == 0.0d && color.B == 1.0d, "Color is not purple.");
        }

        [TestMethod]
        public void TestWhiteColor()
        {
            Color color = Color.White;

            Assert.IsTrue(color.R == 1.0d && color.G == 1.0d && color.B == 1.0d, "Color is not white.");
        }

        [TestMethod]
        public void TestBlackColor()
        {
            Color color = Color.Black;

            Assert.IsTrue(color.R == 0.0d && color.G == 0.0d && color.B == 0.0d, "Color is not black.");
        }

        [TestMethod]
        public void TestEqualColor()
        {
            Color A = Color.Red;
            Color B = Color.Red;
            Color C = Color.Blue;
            
            Assert.IsTrue(A.Equals(B),  "Color A should be equal to color B.");
            Assert.IsFalse(A.Equals(C), "Color A should not be equal to color C.");
        }

        [TestMethod]
        public void TestOperatorMultiplyScalarColor()
        {
            Color lhs = new Color(2.0d, 3.0d, 4.0d);
            double rhs = 5.0d;
            Color mtp = lhs * rhs;

            Assert.IsTrue(mtp.R == 10.0d, "Vector X component did not multiply correctly.");
            Assert.IsTrue(mtp.G == 15.0d, "Vector Y component did not multiply correctly.");
            Assert.IsTrue(mtp.B == 20.0d, "Vector Z component did not multiply correctly.");
        }

        [TestMethod]
        public void TestOperatorMultiplyColor()
        {
            Color lhs = new Color(2.0d, 3.0d, 4.0d);
            Color rhs = new Color(3.0d, 4.0d, 5.0d);
            Color mtp = lhs * rhs;

            Assert.IsTrue(mtp.R == 6.0d,    "Vector X component did not multiply correctly.");
            Assert.IsTrue(mtp.G == 12.0d,   "Vector Y component did not multiply correctly.");
            Assert.IsTrue(mtp.B == 20.0d,   "Vector Z component did not multiply correctly.");
        }

        [TestMethod]
        public void TestOperatorAddColor()
        {
            Color lhs = new Color(2.0d, 3.0d, 4.0d);
            Color rhs = new Color(3.0d, 4.0d, 5.0d);
            Color add = lhs + rhs;

            Assert.IsTrue(add.R == 5.0d, "Vector X component did not add correctly.");
            Assert.IsTrue(add.G == 7.0d, "Vector Y component did not add correctly.");
            Assert.IsTrue(add.B == 9.0d, "Vector Z component did not add correctly.");
        }

        [TestMethod]
        public void TestMixColor()
        {
            Color A = Color.Black;
            Color B = Color.White;

            Color none            = Color.Mix(A, B, 0.0d);
            Color quarter         = Color.Mix(A, B, 0.25d);
            Color half            = Color.Mix(A, B, 0.5d);
            Color threeQuarter    = Color.Mix(A, B, 0.75d);
            Color full            = Color.Mix(A, B, 1.0d);

            Assert.IsTrue(none.Equals(Color.Black),                             "Mix none failed.");
            Assert.IsTrue(quarter.Equals(new Color(0.25d, 0.25d, 0.25d)),       "Mix quarter failed.");
            Assert.IsTrue(half.Equals(new Color(0.5d, 0.5d, 0.5d)),             "Mix half failed.");
            Assert.IsTrue(threeQuarter.Equals(new Color(0.75d, 0.75d, 0.75d)),  "Mix three quarter failed.");
            Assert.IsTrue(full.Equals(Color.White),                             "Mix full failed.");
        }

        [TestMethod]
        public void TestGammaCorrectionColor()
        {
            Color color = new Color(0.9d, 0.3d, 0.6d);
            color = Color.GammaCorrection(color);

            // Approximate the gamma values to account for any precision errors
            Assert.IsTrue(color.R > 0.95d && color.R < 0.96d, "Red component was not correctly gamma-corrected.");
            Assert.IsTrue(color.G > 0.57d && color.G < 0.58d, "Green component was not correctly gamma-corrected.");
            Assert.IsTrue(color.B > 0.79d && color.B < 0.80d, "Blue component was not correctly gamma-corrected.");
        }
    }
}
