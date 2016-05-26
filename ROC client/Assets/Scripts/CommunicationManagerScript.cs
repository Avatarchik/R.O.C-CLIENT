using UnityEngine;
using System.Net.Sockets;
using System;

public class CommunicationManagerScript : MonoBehaviour {

    //List of GameObjects needed
    private MovementScript movementScript = null;
    private PositionScript positionScript = null;
    private GPSScript gpsScript = null;

    private const int headerMask = 0xff;
    private const int typeCmd = 1 << 6;
    private const int typeData = 1 << 7;
    private const int typeError = typeCmd | typeData;


    private string ip = "127.0.0.1";
    private int port = 5050;
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
        byte[] inStream = new byte[128];

        if (goLink.Client.Available != 0)
        {
            CheckInputStream();
//            Debug.Log((int)goLink.ReceiveBufferSize);
  //          serverStream.Read(inStream, 0, (int)goLink.ReceiveBufferSize);
          //  string returndata = System.Text.Encoding. .GetString(inStream);
  //          Debug.Log("Receiving from go -> :" + BitConverter.ToString(inStream));
        }
    }
   
    public void CheckInputStream()
    {
        byte[] inStream = new byte[128];

        goLink.Client.Receive(inStream);
        Debug.Log("Receiving from go -> :" + BitConverter.ToString(inStream));

        switch (inStream[1])
        {
            case typeData:
                AnalyseData(inStream);
                break;
            case typeError:
                AnalyseError(inStream);
                break;
            case typeCmd:
                AnalyseCmd(inStream);
                break;
            default:
                break;
        }
    }

    public void AnalyseData(byte[] inStream)
    {
        if (inStream[2] == 0xb)
            SetGpsCoordinates((float)BitConverter.ToDouble(inStream,3), (float)BitConverter.ToDouble(inStream, 7));
    }

    public void AnalyseError(byte[] inStream)
    {

    }

    public void AnalyseCmd(byte[] inStream)
    {

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

        return 1;
    } 

    public void StopGoLink()
    {
        goLink.Close();
    }

    private void SetCameraAxis()
    {
        float rotationSpeed = 0.5F;

        if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
            return;

        if (positionScript != null)
            positionScript.RotateModel(Input.GetAxis("Vertical") * rotationSpeed, Input.GetAxis("Horizontal") * rotationSpeed);
        SendHeadAngles(positionScript.getXAngle(), positionScript.getYAngle());
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
            gpsScript.InitGps();
            this.SetGpsCoordinates((float)48.8505248, (float)2.346743);
        }
    }

    private void OnApplicationQuit()
    {
        this.StopGoLink();
    }

    public bool SetGpsCoordinates(float x, float y)
    {
        gpsScript.SetGpsMap(x, y);
        gpsScript.RefreshGps();
        return true;
    }

    private byte[] GetBasePacket()
    {
        byte[] outStream = new byte[128];
        for (int i = 0; i < outStream.Length; i++)
            outStream[i] = 0;

        outStream[0] = 0xAF; // magic
        outStream[1] = Convert.ToByte("1010001", 2); // header
 
        return outStream;
    }

    public void SendCoordinates(float lon, float lat)
    {
        byte[] outStream = GetBasePacket();

        outStream[2] = 0xB; //GPS packet

        byte[] lonByte = BitConverter.GetBytes(lon); //longitude
        Buffer.BlockCopy(lonByte, 0, outStream, 3, lonByte.Length);
        byte[] latByte = BitConverter.GetBytes(lat);// latitude
        Buffer.BlockCopy(latByte, 0, outStream, 3 + lonByte.Length, latByte.Length);

        SendPacketToGo(outStream);
    }

    public void SendHeadAngles(float x, float y)
    {
        byte[] outStream = GetBasePacket();

        outStream[2] = 0xA; //Head packet

        byte[] xByte = BitConverter.GetBytes(x); //longitude
        Buffer.BlockCopy(xByte, 0, outStream, 3, xByte.Length);
        byte[] yByte = BitConverter.GetBytes(y);// latitude
        Buffer.BlockCopy(yByte, 0, outStream, 3 + xByte.Length, yByte.Length);

        SendPacketToGo(outStream);
    }

    public void MoveHead()
    {

    }

    private void SendPacketToGo(byte[] array)
    {
        Debug.Log("Sending to go -> :" + BitConverter.ToString(array));
        goLink.Client.Send(array);

    }
}
