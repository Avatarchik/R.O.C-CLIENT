using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

public class CanvasManagerScript : MonoBehaviour {

    bool DEVSTATUs = false;

    GameObject canvasObj;

    // Use this for initialization
    void Start() {
        canvasObj = GameObject.Find("RawImage");
    }

    // Update is called once per frame
    void Update() {

    }

    // Set the canvas image to the corresponding Mat
    public void SetImage(Mat mat)
    {
        Debug.Log("ta soeur");
        Texture2D texture = TextureConvert.ImageToTexture2D(mat.ToImage<Bgr, Byte>(), FlipType.Vertical);

        canvasObj.GetComponent<RawImage>().texture = texture;
        //GameObject.Find("Image").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }


    /*        if (DEVSTATUs == true)
            {
                return;
            }

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
        }*/



    //TODO : DELETE AFTER DEV
    /*
    public void openNewWindow()
    {
        DEVSTATUs = true;
        Debug.Log("init receive 1");
        VideoCapture capture = new VideoCapture(0);
        if (!capture.IsOpened())
            Debug.Log("Failed to open camera");
        int sleepTime = (int)Math.Round(1000 / capture.Fps);
        Debug.Log("init receive 2");

        using (var window = new Window("capture"))
        {
            // Frame image buffer
            Mat image = new Mat();

            int i = 0;
            while (i != 27)
            {
                capture.Grab();
                NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(capture.CvPtr, image.CvPtr);
                //  capture.Read(image); // same as cvQueryFrame
                Debug.Log("init receive 3");
                if (image.Empty())
                    break;
                Debug.Log("init receive 4");

                window.ShowImage(image);
                i = Cv2.WaitKey(10);
            }
          window.Close();
            DEVSTATUs = false;
        }
    }*/
}
