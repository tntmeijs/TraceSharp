using Microsoft.VisualStudio.TestTools.UnitTesting;

using PathTracer.Rendering;
using System.IO;

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

        [TestMethod]
        public void TestSavePpmImage()
        {
            const string SAVE_DIR   = "./";
            const string FILE_NAME  = "ppm_export_unit_test.ppm";

            const int WIDTH = 64;
            const int HEIGHT = 64;

            string fullPath = Path.GetFullPath(Path.Combine(SAVE_DIR, FILE_NAME));

            Image image = new Image(WIDTH, HEIGHT);

            // Set all pixels to green
            for (int i = 0; i < WIDTH * HEIGHT; ++i)
            {
                image.SetPixel(i, Color.Green);
            }

            // Write to a file
            image.SaveToPpm(SAVE_DIR, FILE_NAME);

            Assert.IsTrue(File.Exists(fullPath),                "File does not exist.");
            Assert.IsTrue(new FileInfo(fullPath).Length > 0,    "Image file does not have any data.");

            // Remove the file to ensure that the directory is left the way it was before the test
            File.Delete(fullPath);
        }
    }
}
