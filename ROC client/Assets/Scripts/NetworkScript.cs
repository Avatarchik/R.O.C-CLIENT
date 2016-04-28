using UnityEngine;
using System;
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

    private CaptureJob captureJob1 = null;
    private CaptureJob captureJob2 = null;
    private bool mainSceneLoaded = false;
    EventWaitHandle _wh1 = new AutoResetEvent(false);
    EventWaitHandle _wh2 = new AutoResetEvent(false);


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
        //TODO fps optimisation setCaptureProperty
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
            {
                captureJob1 = new CaptureJob(0, _wh1);
                captureJob2 = new CaptureJob(1, _wh2);

                captureJob1.Start();
                captureJob2.Start();
            }
            else
            {
                captureJob1 = new CaptureJob(this.rtspAddr, _wh1);
                captureJob1.Start();
            }
            Debug.Log("CAPTURE DONE");
            Debug.Log("capture= " + captureJob1);
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

    public void ProcessFrame(object sender, EventArgs e)
    {


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


    private void Update() {
        try
        {
            if (mainSceneLoaded == true && captureJob1 != null )
            {
                if (captureJob1.Update() == true)
                {
                    /*  if (canvasScript.canvasImage.texture)
                      {
                          Texture2D.DestroyImmediate(canvasScript.canvasImage.texture, true);
                      }*/
                    //Test de release ....
                    canvasScript.SetImage1(captureJob1.getFrame());
                    // captureJob.releaseFrame();
                    captureJob1.isDone = false;
                    this._wh1.Set();
                }
            }
            if (mainSceneLoaded == true && captureJob2 != null)
            {
                if (captureJob2.Update() == true)
                {
                    /*  if (canvasScript.canvasImage.texture)
                      {
                          Texture2D.DestroyImmediate(canvasScript.canvasImage.texture, true);
                      }*/
                    //Test de release ....
                    canvasScript.SetImage2(captureJob2.getFrame());
                    // captureJob.releaseFrame();
                    captureJob2.isDone = false;
                    this._wh2.Set();
                }
            }
            //if (mainSceneLoaded == true && captureJob1 != null && captureJob2 != null)
            //{
            //    Debug.Log("viens ici");
            //    if (captureJob1.Update() == true && captureJob2.Update() == true)
            //    {
            //      /*  if (canvasScript.canvasImage.texture)
            //        {
            //            Texture2D.DestroyImmediate(canvasScript.canvasImage.texture, true);
            //        }*/
            //        //Test de release ....
            //        canvasScript.SetImage(captureJob1.getFrame(), captureJob2.getFrame());
            //       // captureJob.releaseFrame();
            //        captureJob1.isDone = false;
            //        captureJob2.isDone = false;
            //    }
            //}
            //else if (mainSceneLoaded == true && captureJob1 != null)
            //{
            //    Debug.Log("pk ici ?");
            //    if (captureJob1.Update() == true)
            //    {
            //        /*  if (canvasScript.canvasImage.texture)
            //          {
            //              Texture2D.DestroyImmediate(canvasScript.canvasImage.texture, true);
            //          }*/
            //        //Test de release ....
            //        canvasScript.SetImage(captureJob1.getFrame(), captureJob1.getFrame());
            //        // captureJob.releaseFrame();
            //        captureJob1.isDone = false;
            //    }
            //}
        }
        catch (Exception e)
        {
            Debug.Log("ERROR : QueryFrame Exception.");
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
        if (captureJob1 != null)
        {
            captureJob1.Abort();
            captureJob1 = null;
        }
        if (captureJob2 != null)
        {
            captureJob2.Abort();
            captureJob2 = null;
        }
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
