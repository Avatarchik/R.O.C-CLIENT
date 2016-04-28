using UnityEngine;
using NUnit.Framework;
using Emgu.CV;

[TestFixture]
[Category("Canvas Manager Tests")]
class CanvasManagerTest
{
    CanvasManagerScript script = new CanvasManagerScript();

    [Test]
    [Category("Unknown Tests")]
    public void SetImageTest()
    {
        Mat testMat = new Mat();

        //Assert.AreEqual(true, script.SetImage(testMat), "Mat could not be converted to Texture2D");
    }
}
