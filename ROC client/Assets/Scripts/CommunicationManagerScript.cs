using UnityEngine;
using System.Net.Sockets;
using System;

public class CommunicationManagerScript : MonoBehaviour {

    //List of GameObjects needed
    private MovementScript movementScript = null;
    private PositionScript positionScript = null;
    private GPSScript gpsScript = null;

    private string ip = "127.0.0.1";
    private int port = 37807;
    private NetworkStream serverStream;
    private TcpClient goLink = new TcpClient();


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start () {
    }

	private void Update () {
        SetCameraAxis();
        SetRobotAxis();

        //TODO reimplement golink writing and reading
        byte[] inStream = new byte[120000];

        if (serverStream != null && serverStream.DataAvailable == true)
        {
            serverStream.Read(inStream, 0, (int)goLink.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            Debug.Log(returndata);
        }
    }

    public int StartGoLink()
    {
        try
        {
            Debug.Log("CONNECTION : Try connect GO with : " + this.ip + " , " + this.port);
            goLink.Connect(this.ip, this.port);
        }
        catch (Exception e)
        {
            Debug.Log("ERROR : " + e.Message);
            return 0; //TODO change to -1 once the go software is correctly ready
        }
        serverStream = goLink.GetStream();
        byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Client packets");
        serverStream.Write(outStream, 0, outStream.Length);
        return 1;
    } 

    public void StopGoLink()
    {
        goLink.Close();
    }

    private void SetCameraAxis()
    {
        float rotationSpeed = 0.5F;
        if (positionScript != null)
            positionScript.RotateModel(Input.GetAxis("Vertical") * rotationSpeed, Input.GetAxis("Horizontal") * rotationSpeed);
    }

    private void SetRobotAxis()
    {
        float rotationSpeed = 0.5F;
        if (movementScript != null)
        {
            movementScript.RotateVehicleModel(Input.GetAxis("VerticalMovement") * rotationSpeed);
            movementScript.RotateCameraModel(Input.GetAxis("Horizontal") * rotationSpeed);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            movementScript = null;
            positionScript = null;
        }
        if (level == 2)
        {
            movementScript = GameObject.Find("MovementObject").GetComponent<MovementScript>();
            positionScript = GameObject.Find("PositionObject").GetComponent<PositionScript>();
            gpsScript = GameObject.Find("GPSObject").GetComponent<GPSScript>();
            this.SetGpsCoordinates((float)48.8505248, (float)2.346743);
        }
    }

    private void OnApplicationQuit()
    {
        this.StopGoLink();
    }

    private void SetGpsCoordinates(float x, float y)
    {
        gpsScript.SetGpsMap(x, y);
        gpsScript.RefreshGpsPosition();
    }
}
