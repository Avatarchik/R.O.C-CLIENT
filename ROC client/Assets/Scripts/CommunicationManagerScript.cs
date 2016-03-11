using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;

public class CommunicationManagerScript : MonoBehaviour {

    NetworkStream serverStream;
   TcpClient goLink;

    Socket test;
    // Use this for initialization
    void Start () {
        goLink = new TcpClient();
        try
        {
            goLink.Connect("127.0.0.1", 37807);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return ;
        }
        serverStream = goLink.GetStream();
        Debug.Log((int)goLink.ReceiveBufferSize);
        byte[] outStream = System.Text.Encoding.ASCII.GetBytes("yolo$");
        serverStream.Write(outStream, 0, outStream.Length);
    }
	
	// Update is called once per frame
	void Update () {
        byte[] inStream = new byte[120000];

        if (serverStream != null && serverStream.DataAvailable == true)
        {
            Debug.Log("swag");
            serverStream.Read(inStream, 0, (int)goLink.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            Debug.Log(returndata);
        }
        }
}
