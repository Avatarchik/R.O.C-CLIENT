using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;

using Emgu.CV.CvEnum;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class NetworkScript : MonoBehaviour
{
    private String ip = "92.144.127.214"; //ip internet
    private IPAddress udpAddr;
    private UdpClient udpClientReceiver;
    private UdpClient udpClientSender;
    private TcpClient tcpClientSender;
    private NetworkStream tcpStream;
    private int port = 1250;
    private Capture _capture;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("toto");
    }
    
    

    public int SetUpNetwork()
    {
        this.ip = GameObject.Find("IpField").GetComponent<InputField>().text;
        this.port = Int32.Parse(GameObject.Find("PortField").GetComponent<InputField>().text);
        Debug.Log("Try connect sender with :\nIP : " + this.ip + "\nPORT : " + this.port);

        _capture = new Capture(0);

        //String win1 = "Test Window"; //The name of the window
        //CvInvoke.NamedWindow(win1); //Create the window using the specific name
        //Mat frame = new Mat();

        //_capture.Retrieve(frame, 0);

        

        //CvInvoke.Imshow(win1, frame); //Show the image
        //CvInvoke.WaitKey(0);  //Wait for the key pressing event
        //CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed
        //_capture.ImageGrabbed += capture_ImageGrabbed;
        return (0);
    }

    private void capture_ImageGrabbed(object sender, EventArgs e)
    {
       
    }

    void Update()
    {

        Mat frame = new Mat();
        _capture.Retrieve(frame, 0);
        GameObject.Find("MainSceneManager").GetComponent<CanvasManagerScript>().SetImage(frame);
    }
    private void ProcessFrame(object sender, EventArgs arg)
    {
    }
    //void videoReceive()
    //{
    //    Debug.Log("init receive 1");
    //    Capture capture = new Capture(0);

    //    Debug.Log("init receive 2");
    //    private Texture2D cameraTex;
    //    cameraTex = TextureConvert.ImageToTexture2D<Bgr, byte>(frame, true);

    //    using (var window = new Window("capture"))
    //    {
    //        // Frame image buffer
    //        OpenCvSharp.Mat image = new OpenCvSharp.Mat();
    //        Image
    //        int i = 0;
    //        while (i != 27)
    //        {
    //            capture.Grab();
    //            NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(capture.CvPtr, image.CvPtr);
    //          //  capture.Read(image); // same as cvQueryFrame
    //            Debug.Log("init receive 3");
    //            if (image.Empty())
    //                break;
    //            Debug.Log("init receive 4");

    //             window.ShowImage(image);
    //            i = Cv2.WaitKey(10);
    //        }
    //    }
    //}

    //void Update()
    //{
    //    if (videoStreamInitialized == true)
    //    {
    //        if (!sceneInitialized)
    //            CheckSceneInitializing();
    //        else
    //            CheckNewFrame();
    //    }
    //}

    void OnDestroy()
    {
        Debug.Log("On destroy called");
    }

    //  Made by Max to work 
    //  TODO : Alex check si tout ca te va
    private bool videoStreamInitialized = false;
    private bool sceneInitialized = false;

    private void CheckSceneInitializing()
    {
        //if (GameObject.Find("MainSceneManager") != null)
        //{
        //    sceneInitialized = true;
        //    CheckNewFrame();
        //}
        //else
        //{
        //    Debug.Log("Main Scene not yet Loaded");
        //}
    }

    //private void CheckNewFrame()
    //{
    //    Mat image = new Mat();

    //    capture.Grab();
    //    NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(capture.CvPtr, image.CvPtr);
    //    /**
    //    if (image.Empty())
    //        break;
    //    **/
    //    Debug.Log("1");
    //    GameObject.Find("MainSceneManager").GetComponent<CanvasManagerScript>().SetImage(image);
    //}
}
