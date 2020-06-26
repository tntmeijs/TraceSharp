using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Rendering;

namespace UnitTests.Rendering
{
    [TestClass]
    public class TestImage
    {
        [TestMethod]
        public void TestInitializeImage()
        {
            Image image = new Image(1920, 1080);

            Assert.IsTrue(image.Width   == 1920, "Image width set incorrectly.");
            Assert.IsTrue(image.Height  == 1080, "Image height set incorrectly.");
        }

        [TestMethod]
        public void TestGetSetPixelImage()
        {
            Image image = new Image(1920, 1080);

            Color initial = image.GetPixel(10);
            image.SetPixel(10, Color.Blue);
            Color current = image.GetPixel(10);

            Assert.IsTrue(initial.Equals(Color.Black),   "Pixel is not set to the correct initial color.");
            Assert.IsTrue(current.Equals(Color.Blue),    "Pixel is not set to the correct new color.");
        }
    }
}
