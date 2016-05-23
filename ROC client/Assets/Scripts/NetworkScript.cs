using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using Emgu.CV;
using Assets.Src;

public class NetworkScript : MonoBehaviour
{
    private CanvasManagerScript canvasScript;

    //TODO IP and port has to be class members ??? only used in one function
    private String ip = "127.0.0.1"; //ip internet
    private int port = 0;
    private String rtspAddr = null;
    private int _nbCamera = 2;

    private List<CaptureJob> _captureList = null;
    private bool mainSceneLoaded = false;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Debug.Log("MANAGER : NetworkScript started");
        Debug.Log("MANAGER : Loading MenuScene");
        Application.LoadLevel("MenuScene");
    }

    //Method called on connect button click, returns -1 upon failure and 0 upon success
    public int SetUpNetwork(string ip, int port)
    {
        //TODO fps optimisation setCaptureProperty
        this.ip = ip;
        this.port = port;
        Debug.Log("CONNECTION : Try connect sender with :\nIP : " + this.ip + "\nPORT : " + this.port);
        if (this.ip == "127.0.0.1" && this.port == 0)
        {
            this.rtspAddr = null;
        }
        else
        {
            this.rtspAddr = "rtsp://" + this.ip + ":";
            Debug.Log("CONNECTION : Rtsp address is set : " + this.rtspAddr);
        }
        try
        {
            CvInvoke.UseOpenCL = false;
            _captureList = new List<CaptureJob>();
            for (int i = 0; i < _nbCamera; i++) // Loop with for.
            {
                this.port += i;
                if (this.rtspAddr == null)
                    _captureList.Add(new CaptureJob(i));
                else
                    _captureList.Add(new CaptureJob(this.rtspAddr + this.port + "/camera_" + i));
            }
            foreach (CaptureJob captureJob in _captureList)
            {
                captureJob.Start();
            }
            Debug.Log("CAPTURE DONE");
        }
        catch (NullReferenceException excpt)
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
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("LEVEL : Level " + level + " loaded.");
        if (level == 1)
        {
            mainSceneLoaded = false;
            this.clearCamera();
        }
        else if (level == 2)
        {
            mainSceneLoaded = true;
            canvasScript = GameObject.Find("MainSceneManager").GetComponent<CanvasManagerScript>();
        }
    }


    private void Update()
    {
        try
        {
            if (mainSceneLoaded == true)
            {
                for (int i = 0; i < _captureList.Count; i++) // Loop with for.
                {
                    if (_captureList[i].Update() == true)
                    {
                        canvasScript.SetImage(i, _captureList[i].getFrame());
                        // captureJob.releaseFrame();
                        _captureList[i].isDone = false;
                        _captureList[i].NewItemEvent.Set();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("ERROR : QueryFrame Exception." + e.Message);
        }
    }

    // Function automatically called once the application exits, frees the camera
    private void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        this.clearCamera();
    }

    //Function releasing the camera streams
    private void clearCamera()
    {
        Debug.Log("INFO : Capture reset.");
        if (_captureList != null)
        {
            for (int i = 0; i < _captureList.Count; i++) // Loop with for.
            {
                _captureList[i]._exitThreadEvent.Set();
                _captureList[i].ReleaseCamera();
            }
        }
        _captureList = null;
        this.ip = "127.0.0.1";
        this.port = 0;
        this.rtspAddr = null;
    }

    public string GetRtspAddr()
    {
        if (this.rtspAddr == null)
            return "Camera 0";
        else
            return this.rtspAddr;
    }

    public bool GetMainLevel()
    {
        return this.mainSceneLoaded;
    }

}
