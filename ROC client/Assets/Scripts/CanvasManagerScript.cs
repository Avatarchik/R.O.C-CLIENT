using UnityEngine;
using UnityEngine.UI;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

public class CanvasManagerScript : MonoBehaviour {

    // List of GameObjects needed
    private List<RawImage> _canvasImage = null;
    private int nbImage;

    void Start() {
        nbImage = 2;
        _canvasImage = new List<RawImage>();
        _canvasImage.Add(GameObject.Find("RawImage").GetComponent<RawImage>());
        _canvasImage.Add(GameObject.Find("RawImage2").GetComponent<RawImage>());
    }

    void Update() {
    }

    // Set the canvas image to the corresponding Mat
    public Boolean SetImage(int index, Mat mat)
    {
        Texture2D temp = TextureConvert.ImageToTexture2D(index, mat.ToImage<Bgr, Byte>(), FlipType.Vertical);

        if (temp == null)
            return false;
        else
        {
            _canvasImage[index].texture = temp;
            return true;
        }
        return false;
    }

    public Boolean SetImage(Mat mat, Mat mat2)
    {
        Texture2D temp = TextureConvert.ImageToTexture2D(0, mat.ToImage<Bgr, Byte>(), FlipType.Vertical);
        Texture2D temp2 = TextureConvert.ImageToTexture2D(1, mat2.ToImage<Bgr, Byte>(), FlipType.Vertical);

        if (temp == null || temp2 == null)
            return false;
        else
        {
            //canvasImage.texture = temp;
            //canvasImage2.texture = temp2;
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
