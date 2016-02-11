using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using System;
using System.Collections;

public class CanvasManagerScript : MonoBehaviour {

    GameObject canvasObj;

    // Use this for initialization
    void Start() {
        canvasObj = GameObject.Find("Image");
    }

    // Update is called once per frame
    void Update() {

    }

    // Set the canvas image to the corresponding Mat
    public void SetImage(Mat mat)
    {
        if (mat.Height != 1920 || mat.Width != 1080)
            mat = ResizeMatTo1920x1080(mat);
        Debug.Log(System.Environment.Version);
        Texture2D tempTexture = new Texture2D(mat.Width, mat.Height);
        tempTexture.LoadImage(MatToByteArray(mat));
        canvasObj.GetComponent<Image>().sprite = Sprite.Create(tempTexture, new UnityEngine.Rect(0, 0, 1920, 1080), Vector2.zero);

        GameObject.Find("TestText").GetComponent<Text>().text = ((int)(1.0f / Time.deltaTime)).ToString();
    }

    private Mat ResizeMatTo1920x1080(Mat mat)
    {
        Size size = new Size(1920, 1080);

        return mat.Resize(size);
    }

    private byte[] MatToByteArray(Mat mat) {
        return (mat.ToBytes(".png"));
    }

}
