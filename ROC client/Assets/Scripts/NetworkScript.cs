using UnityEngine;
using System;
using Emgu.CV;

public class NetworkScript : MonoBehaviour
{
    private CanvasManagerScript canvasScript;

    //TODO IP and port has to be class members ??? only used in one function
    private String ip = "127.0.0.1"; //ip internet
    private int port = 0;
    private String rtspAddr = null;

    private Capture captureVideo = null;
    private Mat frame = new Mat();
    private bool mainSceneLoaded = false;


    private void Awake() {
        DontDestroyOnLoad(this);
    }

    private void Start() {
        Debug.Log("MANAGER : NetworkScript started");
        Debug.Log("MANAGER : Loading MenuScene");
        Application.LoadLevel("MenuScene");
    }

    //Method called on connect button click, returns -1 upon failure and 0 upon success
    public int SetUpNetwork(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
        Debug.Log("CONNECTION : Try connect sender with :\nIP : " + this.ip + "\nPORT : " + this.port);
        if (this.ip.Length > 5 && this.port.ToString().Length > 2) { //TODO change implementation of main camera show before release 
            this.rtspAddr = "rtsp://" + this.ip + ":" + this.port + "/camera_0";
            Debug.Log("CONNECTION : Rtsp address is set : " + this.rtspAddr);
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
            Debug.Log("ERROR : Capture failed : " + excpt.Message);
            this.clearCamera();
            return (-1);
        }
        return (0);
    }

    public void ResetNetwork()
    {
        this.clearCamera();
    }

    // Function automatically called on every script after a level has been loaded
    private void OnLevelWasLoaded(int level) {
        Debug.Log("LEVEL : Level " + level + " loaded.");
        if (level == 1) {
            mainSceneLoaded = false;
            this.clearCamera();
        }
        else if (level == 2) {
            mainSceneLoaded = true;
            canvasScript = GameObject.Find("MainSceneManager").GetComponent<CanvasManagerScript>();
        }
    }

    //Function releasing the camera streams
    private void clearCamera()
    {
        Debug.Log("INFO : Capture reset.");
        if (captureVideo != null)
        {
            captureVideo.Dispose();
            captureVideo = null;
        }
        this.ip = "127.0.0.1";
        this.port = 0;
        this.rtspAddr = null;
    }

    private void Update() {
        if (mainSceneLoaded == true && captureVideo != null) {
            if ((frame = captureVideo.QueryFrame()) != null) {
                canvasScript.SetImage(frame);
            }
            else {
                Debug.Log("ERROR : QueryFrame failed.");
            }
        }
    }

    // Function automatically called once the application exits, frees the camera
    private void OnApplicationQuit()
    {
        this.clearCamera();
    }

    public string GetRtspAddr()
    {
        if (this.rtspAddr == null)
            return "Camera 0";
        else
            return this.rtspAddr;
    }
}
