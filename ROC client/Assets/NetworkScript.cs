using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Collections.Generic;

public class NetworkScript : MonoBehaviour
{
    private UdpClient udpClientReceiver;
    private UdpClient udpClientSender;
    private TcpClient tcpClientSender;
    private NetworkStream tcpStream;
    private int port = 4500;
    private String ip = "92.144.127.42"; //ip internet
    private IPAddress udpAddr;


    public delegate void RequestReceivedEventHandler(string message);
    public event RequestReceivedEventHandler OnRequestReceived;

    // Use this to trigger the event
    protected virtual void ThisRequestReceived(string message)
    {
        RequestReceivedEventHandler handler = OnRequestReceived;
        if (handler != null)
        {
            handler(message);
        }
    }

    // We use this to keep tasks needed to run in the main thread
    private static readonly Queue<Action> tasks = new Queue<Action>();

    // Use this for initialization
    void Start()
    {
        Debug.Log("toto");
        SetUpNetwork("191.168.1.15", 4500);
    }

    public int SetUpNetwork(string ip, int port)
    {
        this.port = port;
        this.ip = ip;
        //Debug.Log("Try connect tcp");
        //if (InitTcpClient() == -1)
        //    return (-1);
        Debug.Log("Try connect sender");
        if (InitUdpClientSend() == -1)
            return (-1);
        Debug.Log("Try connect udp receive");
        if (InitUdpClientReceive() == -1)
            return (-1);
        return (0);
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
            IPEndPoint requestGroupEP = new IPEndPoint(IPAddress.Any, this.port);
            this.udpClientReceiver.Client.Bind(requestGroupEP);
            this.udpClientReceiver.BeginReceive(new AsyncCallback(AsyncUdpReceive), null);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return (-1);
        }
        // Listen for the request
        this.OnRequestReceived += (message) =>
        {
            Debug.Log("Request Received: " + message);
            // Do some more stuff 
        };
        return (0);
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
            string message = "Test Sending";

            if (message != "")
            {
                Debug.Log("Sendering Request: " + message);
                this.udpClientSender.Send(System.Text.Encoding.ASCII.GetBytes(message), message.Length);
            }
        }
        catch (ObjectDisposedException e)
        {
            Debug.LogWarning("Trying to send data on already disposed UdpCleint: " + e);
            return;
        }

    }
    void HandleTasks()
    {
        while (tasks.Count > 0)
        {
            Action task = null;

            lock (tasks)
            {
                if (tasks.Count > 0)
                {
                    task = tasks.Dequeue();
                }
            }

            task();
        }
    }

    public void QueueOnMainThread(Action task)
    {
        lock (tasks)
        {
            tasks.Enqueue(task);
        }
    }

    private void AsyncUdpReceive(IAsyncResult result)
    {
        IPEndPoint receiveIPGroup = new IPEndPoint((System.Net.IPAddress.Parse(this.ip)), this.port);
        byte[] received;
        if (this.udpClientReceiver != null)
        {
            received = this.udpClientReceiver.EndReceive(result, ref receiveIPGroup);
        }
        else
        {
            return;
        }
        this.udpClientReceiver.BeginReceive(new AsyncCallback(AsyncUdpReceive), null);
        string receivedString = System.Text.Encoding.ASCII.GetString(received);


        this.QueueOnMainThread(() =>
        {
            // Fire the event
            this.ThisRequestReceived(receivedString);
        });

    }

    // Update is called once per frame
    void Update()
    {
        this.HandleTasks();
    }
}
