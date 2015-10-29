using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;

public class NetworkScript : MonoBehaviour
{
    private UdpClient udpClientReceiver;
    private UdpClient udpClientSender;
    private TcpClient tcpClientSender;
    private NetworkStream tcpStream;
    private int port = 1250;
    private String ip = "92.144.127.214"; //ip internet
    private IPAddress udpAddr;
    private Thread receiveThread = null;
    private static readonly Queue<Action> tasks = new Queue<Action>();
    private int i = 0;

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


    void Awake()
    {
        DontDestroyOnLoad(this);
    }


    // Use this for initialization
    void Start()
    {
        Debug.Log("toto");
        SetUpNetwork("192.168.1.15", 1250);
    }

    public int SetUpNetwork(string ip, int port)
    {
        this.port = port;
        this.ip = ip;
        Debug.Log("Try connect sender");
        if (InitUdpClientSend() == -1)
            return (-1);
        InitUdpClientReceive();
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

    void InitUdpClientReceive()
    {
        try
        {
            IPEndPoint requestGroupEP = new IPEndPoint(IPAddress.Any, this.port);
            this.udpClientReceiver = new UdpClient(requestGroupEP);
            this.udpClientReceiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Debug.Log("Try receive");

            this.udpClientReceiver.BeginReceive(new AsyncCallback(AsyncUdpReceive),  null);
            Debug.Log("Begin Receive");

            this.OnRequestReceived += (message) => {
                Debug.Log("Request Received: " + message.Length);

            };
            Debug.Log("TOTO");
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
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
                Debug.Log("MON MSGGGGG " +  received.Length);
            }
            else
            {
                return;
            }
            this.udpClientReceiver.BeginReceive(new AsyncCallback(AsyncUdpReceive), null);
            string receivedString = System.Text.Encoding.ASCII.GetString(received);
            this.QueueOnMainThread(() => {
                // Fire the event
                this.ThisRequestReceived(receivedString);
            });
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
        i++;
        this.HandleTasks();
    }

    public void HandleTasks()
    {
        GameObject.Find("TestText").GetComponent<Text>().text = i.ToString();
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

    void OnDestroy()
    {
        Debug.Log("On destroy called");
        if (receiveThread != null)
            receiveThread.Abort();
        //udpClientReceiver.Close();
        //udpClientSender.Close();
    }
}
