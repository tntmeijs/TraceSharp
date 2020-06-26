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
    }
}
