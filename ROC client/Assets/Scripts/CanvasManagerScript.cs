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
    private RawImage canvasImage2;

    void Start() {
        canvasImage = GameObject.Find("RawImage").GetComponent<RawImage>();
        canvasImage2 = GameObject.Find("RawImage2").GetComponent<RawImage>();
    }

    void Update() {
    }

    // Set the canvas image to the corresponding Mat
    public Boolean SetImage1(Mat mat)
    {
        Texture2D temp = TextureConvert.Image1ToTexture2D(mat.ToImage<Bgr, Byte>(), FlipType.Vertical);

        if (temp == null)
            return false;
        else
        {
            canvasImage.texture = temp;
            return true;
        }
        return false;
    }

    public Boolean SetImage2(Mat mat)
    {
        Texture2D temp2 = TextureConvert.Image2ToTexture2D(mat.ToImage<Bgr, Byte>(), FlipType.Vertical);

        if (temp2 == null)
            return false;
        else
        {
            canvasImage2.texture = temp2;
            return true;
        }
        return false;
    }

    public Boolean SetImage(Mat mat, Mat mat2)
    {
        Texture2D temp = TextureConvert.Image1ToTexture2D(mat.ToImage<Bgr, Byte>(), FlipType.Vertical);
        Texture2D temp2 = TextureConvert.Image2ToTexture2D(mat2.ToImage<Bgr, Byte>(), FlipType.Vertical);

        if (temp == null || temp2 == null)
            return false;
        else
        {
            canvasImage.texture = temp;
            canvasImage2.texture = temp2;
            return true;
        }
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
