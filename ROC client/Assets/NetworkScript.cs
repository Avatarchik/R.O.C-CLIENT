using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;

public class NetworkScript : MonoBehaviour
{
    private UdpClient udpClientReceiver;
    private UdpClient udpClientSender;
    private TcpClient tcpClientSender;
    private NetworkStream tcpStream;
    private int port = 4500;
    private String ip = "92.144.127.42"; //ip internet
    private IPAddress udpAddr;
    private Thread receiveThread;

    // Use this for initialization
    void Start()
    {
        Debug.Log("toto");
        SetUpNetwork("92.144.127.42", 1250);
    }

    public int SetUpNetwork(string ip, int port)
    {
        this.port = port;
        this.ip = ip;
        Debug.Log("Try connect sender");
        if (InitUdpClientSend() == -1)
            return (-1);
        receiveThread = new Thread(
    new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log("Try connect udp receive");
        //if (InitUdpClientReceive() == -1)
        //    return (-1);
        return (0);
    }

    private void ReceiveData()
    {
        udpClientReceiver = new UdpClient();
        while (true)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClientReceiver.Receive(ref RemoteIpEndPoint);

                string returnData = Encoding.ASCII.GetString(receiveBytes);

                Debug.Log("This is the message you received " +
                                             returnData.ToString());
                Debug.Log("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    int InitTcpClient()
    {
        tcpClientSender = new TcpClient();
        tcpClientSender.Connect(this.ip, this.port);
        tcpStream = tcpClientSender.GetStream();
        if (tcpStream == null)
            return (-1);
        return (0);
    }

    int SendTcpPacket(String msg)
    {
        if (tcpStream.CanWrite)
        {
            Byte[] sendBytes = Encoding.UTF8.GetBytes(msg);
            tcpStream.Write(sendBytes, 0, sendBytes.Length);
        }
        else
            return (1);
        return (0);
    }

    int InitUdpClientReceive()
    {
        try
        {
            this.udpClientReceiver = new UdpClient();
            this.udpClientReceiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Debug.Log("Try new Ipendpoint");
            IPEndPoint requestGroupEP = new IPEndPoint(IPAddress.Any, this.port);
            Debug.Log("Try bind");
            this.udpClientReceiver.Client.Bind(requestGroupEP);
            Debug.Log("Try receive");
            this.udpClientReceiver.BeginReceive(new AsyncCallback(AsyncUdpReceive), null);
            Debug.Log("Begin Receive");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return (-1);
        }
        return (0);
    }

    private void AsyncUdpReceive(IAsyncResult result)
    {
        try
        {
            Debug.Log("Start Async Task");
            Debug.Log("Try new Ipendpoint");
            IPEndPoint receiveIPGroup = new IPEndPoint(IPAddress.Any, this.port);
            byte[] received;
            if (this.udpClientReceiver != null)
            {
                received = this.udpClientReceiver.EndReceive(result, ref receiveIPGroup);
                Debug.Log("MON MSGGGGG = " + received);
            }
            else
            {
                return;
            }
            string receivedString = System.Text.Encoding.ASCII.GetString(received);
            this.udpClientReceiver.BeginReceive(new AsyncCallback(AsyncUdpReceive), null);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    int InitUdpClientSend()
    {
        try
        {
            this.udpAddr = IPAddress.Parse(this.ip);
            // Set up the sender for requests
            this.udpClientSender = new UdpClient();
            IPEndPoint requestGroupEP = new IPEndPoint(this.udpAddr, this.port);
            this.udpClientSender.Connect(requestGroupEP);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return (-1);
        }
        // Send out the request
        this.SendRequest();
        return (0);
    }

    void SendRequest()
    {
        try
        {
            string message = "Test Toto";

            if (message != "")
            {
                Debug.Log("IP: " + this.ip);
                Debug.Log("PORT: " + this.port);
                Debug.Log("Sendering Request: " + message + "SIZE = " + message.Length.ToString());
                this.udpClientSender.Send(System.Text.Encoding.ASCII.GetBytes(message), message.Length);
                Debug.Log("Send Done");
            }
        }
        catch (ObjectDisposedException e)
        {
            Debug.LogWarning("Trying to send data on already disposed UdpCleint: " + e);
            return;
        }

    }

    void Update()
    {
    }
}
