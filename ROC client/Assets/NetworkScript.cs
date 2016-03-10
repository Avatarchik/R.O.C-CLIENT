using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
    
using Emgu.CV.CvEnum;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class NetworkScript : MonoBehaviour
{
    private String ip = "127.0.0.1"; //ip internet
    private int port = 0;
    private String rtspAddr = null;
    private Capture captureVideo = null;
    private Mat frame;
    private bool mainSceneLoaded;
    private CanvasManagerScript canvasScript;


    void Awake() {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start() {
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
        if (this.ip.Length > 5 && this.port.ToString().Length > 2)
        {
            this.rtspAddr = "rtsp://" + this.ip + ":" + this.port + "/camera_0";
            Debug.Log("rtsp addr is set : " + this.rtspAddr);
        }
        try
        {
            if (this.rtspAddr == null)
                captureVideo = new Capture(0);
            else
                captureVideo = new Capture(this.rtspAddr);
        }
        catch (Exception excpt)
        {
            //EditorUtility.DisplayDialog("Error", excpt.Message, "ok", "");
            Debug.Log("capture failed : " + excpt.Message);
            captureVideo = null;
            return (-1);
            //capture fail retourner au menu
        }
        return (0);
    }

    // Function automatically called on every script after a level has been loaded
    void OnLevelWasLoaded(int level) {
        if (level == 1) {
            canvasScript = GameObject.Find("MainSceneManager").GetComponent<CanvasManagerScript>();
            mainSceneLoaded = true;
        }
        else if (level == 0)
        {
            mainSceneLoaded = false;
            if (captureVideo != null) {
                captureVideo.Dispose();
                captureVideo = null;
                this.ip = "127.0.0.1";
                this.port = 0;
                this.rtspAddr = null;
            }
        }
    }

    void Update() {
        if (mainSceneLoaded == true && captureVideo != null) {
            captureVideo.Retrieve(frame, 0);
            canvasScript.SetImage(frame);
        }
    }

    void OnDestroy() {
        Debug.Log("On destroy called");
    }

}
