using UnityEngine;
using UnityEngine.UI;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing;

public class CanvasManagerScript : MonoBehaviour {

    // List of GameObjects needed
    private RawImage canvasImage;

    void Start() {
        canvasImage = GameObject.Find("RawImage").GetComponent<RawImage>();
    }

    void Update() {
    }

    // Set the canvas image to the corresponding Mat
    public void SetImage(Mat mat)
    {
        canvasImage.texture = TextureConvert.ImageToTexture2D(mat.ToImage<Bgr, Byte>(), FlipType.Vertical);
    }

    // Function converting a CV::Mat to a usable Texture2D
    private Texture2D MatToTexture2d(Bitmap bmp, int texturewidth, int textureheight)
    {
        Texture2D texture = new Texture2D(1920, 1080);
        MemoryStream ms = new MemoryStream();

        bmp.Save(ms, bmp.RawFormat);
        texture.LoadImage(ms.ToArray());
        ms.Close();
        return texture;
    }
}
