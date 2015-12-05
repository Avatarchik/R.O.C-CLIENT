using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using OpenCvSharp;

public class NetworkScript : MonoBehaviour
{
    private UdpClient udpClientReceiver;
    private UdpClient udpClientSender;
    private TcpClient tcpClientSender;
    private NetworkStream tcpStream;
    private int port = 1250;
    private String ip = "92.144.127.214"; //ip internet
    private IPAddress udpAddr;

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

        videoReceive();
        return (0);
    }

    void videoReceive()
    {
        Debug.Log("init receive 1");
        VideoCapture capture = new VideoCapture("rtsp://192.168.1.12:1935/vod/Grimm.mp4");
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
        }
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        Debug.Log("On destroy called");
    }
}
