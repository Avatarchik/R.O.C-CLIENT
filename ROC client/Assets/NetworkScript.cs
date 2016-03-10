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
    private Mat frame;
    private bool mainSceneLoaded;

    private CanvasManagerScript canvasScript;


    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start of Network Script");

        frame = new Mat();
        mainSceneLoaded = false;
    }

    //Method called on connect button click
    public int SetUpNetwork()
    {
        this.ip = GameObject.Find("IpField").GetComponent<InputField>().text;
        this.port = Int32.Parse(GameObject.Find("PortField").GetComponent<InputField>().text);
        Debug.Log("Try connect sender with :\nIP : " + this.ip + "\nPORT : " + this.port);
        _capture = new Capture(0);

        return (0);
    }

    // Function automatically called on every script after a level has been loaded
    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            canvasScript = GameObject.Find("MainSceneManager").GetComponent<CanvasManagerScript>();
            mainSceneLoaded = true;
        }
        else if (level == 0)
        {
            mainSceneLoaded = false;
            _capture.Dispose();
        }
    }

    void Update()
    {
        if (mainSceneLoaded == true)
        {
            _capture.Retrieve(frame, 0);
            canvasScript.SetImage(frame);
        }
    }

    void OnDestroy()
    {
        Debug.Log("On destroy called");
    }

}
