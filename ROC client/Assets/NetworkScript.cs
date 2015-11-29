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
        VideoCapture capture = new VideoCapture("E://Téléchargement//SampleVideo_1080x720_10mb.mp4");


        int sleepTime = (int)Math.Round(1000 / capture.Fps);
        Debug.Log("init receive 2");

        using (var window = new Window("capture"))
        {
            // Frame image buffer
            Mat image = new Mat();

            while (true)
            {
                capture.Grab();
                NativeMethods.videoio_VideoCapture_operatorRightShift_Mat(capture.CvPtr, image.CvPtr);
                //capture.Read(image); // same as cvQueryFrame
                if (image.Empty())
                    break;
                Debug.Log("init receive 3");

                window.ShowImage(image);
                Cv2.WaitKey(sleepTime);
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
