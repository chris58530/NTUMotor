using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading;
using _.Scripts.Json;
using UniRx;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class ConnectUDP : MonoBehaviour
{
    private const string UDP_HOST_IP = "127.0.0.1";
    private const int UDP_HOST_PORT = 7070;
    private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
    private ManualResetEvent _pauseEvent = new ManualResetEvent(true);
    private Thread _thread = null!;
    private static System.Object _sendLock = new System.Object();
    private static Socket _peersock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    public static JsonData JsonData;
    [SerializeField] private GameObject xchartObj;
    private bool _success;

    public void Start()
    {
        // Reset the shutdown event
        _shutdownEvent.Reset();
        _pauseEvent.Set();

        // Initialize Thread
        _thread = new Thread(DoWork);
        _thread.Start();
        Debug.Log("Thread started running");

        Observable.EveryUpdate().Where(_ => _success).First().Subscribe(_ =>
        {
            xchartObj.gameObject.GetComponent<XChartTest>().enabled = true;
        }).AddTo(this);
    }

    private void OnDisable()
    {
        Stop();
    }

    public void Pause()
    {
        /* Sets the state of the event to nonsignaled,
         * causing threads to block.
         */
        _pauseEvent.Reset();
        Debug.Log("Thread paused");
    }

    public void Resume()
    {
        /* Sets the state of the event to signaled,
         * allowing one or more waiting threads to proceed.
         */
        _pauseEvent.Set();
        Debug.Log("Thread resuming");
    }

    public void Stop()
    {
        // close the transmit channel
        if (_peersock != null)
            _peersock.Close();

        // Signal the shutdown event
        _shutdownEvent.Set();

        // Make sure to resume any paused threads
        _pauseEvent.Set();
    }

    private void DoWork()
    {
        IPAddress peerAddr = IPAddress.Parse(UDP_HOST_IP);
        EndPoint _peerep = new IPEndPoint(peerAddr, UDP_HOST_PORT);

        while (true)
        {
            /* 封鎖目前執行緒, 直到waitHandle收到通知,
             * Timeout.Infinite表示無限期等候
             */
            _pauseEvent.WaitOne(Timeout.Infinite);

            /* return true if the current instance receives a signal.
             * If the current instance is never signaled, WaitOne never returns
             */
            if (_shutdownEvent.WaitOne(0))
                break;
            /* if (_shutdownEvent.WaitOne(Timeout.Infinite))
             * 因為沒有收到signal, 所以會停在if()這一行, 造成cnt無法累加
             */

            lock (_sendLock)
            {
                try
                {
                    /*
                     * DataFormat: <BeginAR>{ "CMD" : "QUERY" }<EndAR>
                     */
                    string szCmd = "{ \"CMD\" : \"QUERY\" }";
                    string outstr = "<BeginAR>" + szCmd + "<EndAR>";

                    byte[] sentbytes = Encoding.ASCII.GetBytes(outstr);

                    int sent = _peersock.SendTo(sentbytes, _peerep);
                    if (sent < 0)
                        break;

                    _peersock.ReceiveTimeout = 2000;

                    string retstr = "";
                    byte[] recvbytes = new byte[1024 * 32];

                    while (true)
                    {
                        //Do UDP Receive
                        int recv = _peersock.ReceiveFrom(recvbytes, ref _peerep);
                        if (recv < 0)
                            break;

                        retstr += Encoding.ASCII.GetString(recvbytes, 0, recv);
                        Debug.Log(retstr.Length);

                        if ((retstr.Substring(0, 9) == "<BeginSL>") &&
                            (retstr.Substring(retstr.Length - 7) == "<EndSL>"))
                        {
                            string strjson = retstr.Substring(9, retstr.Length - 16);
                            Debug.Log(strjson);

                            if (strjson.Length == 16) // No Data:   { "Status" : 0 }
                            {
                                Debug.Log("Query Recv = " + strjson.Length.ToString());
                            }
                            else // Simulation Data:  { "Status" : 1, "DataSet": [....] }
                            {
                                // Define FilePath
                                // Write JSON to file

                                Debug.Log("Data Recv = " + strjson.Length.ToString());


                                // Notify Unity3D => Data Received

                                JsonData = JsonUtility.FromJson<JsonData>(strjson);
                                _success = true;
                            }

                            break;
                        }
                    }
                }
                catch (SocketException e)
                {
                    Debug.LogError("SocketException:" + e.Message);
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception:" + e.Message);
                }
            }

            Thread.Sleep(2000);
        }

        Debug.Log("TwinModelConn Thread Stopped ");
    }

    // Call this method when executing command from GUI
    public string SendCommand(string szCmd)
    {
        lock (_sendLock)
        {
            IPAddress peerAddr = IPAddress.Parse(UDP_HOST_IP);
            EndPoint _peerep = new IPEndPoint(peerAddr, UDP_HOST_PORT);

            try
            {
                /*
                 * DataFormat: <BeginAR>{ "CMD" : "Func1" }<EndAR>
                 */
                szCmd = "<BeginAR>" + szCmd + "<EndAR>";
                byte[] sentbytes = Encoding.ASCII.GetBytes(szCmd);

                int sent = _peersock.SendTo(sentbytes, _peerep);
                if (sent < 0)
                    return "";

                byte[] recvbytes = new byte[2048];

                //Do UDP Receive
                int recv = _peersock.ReceiveFrom(recvbytes, ref _peerep);
                if (recv < 0)
                    return "";

                string retstr = Encoding.ASCII.GetString(recvbytes, 0, recv);

                if ((retstr.Substring(0, 9) == "<BeginSL>") && (retstr.Substring(retstr.Length - 7) == "<EndSL>"))
                {
                    string strjson = retstr.Substring(9, retstr.Length - 16);
                    Debug.Log(strjson);

                    /* Command Result
                     * Succeed: { "Status" : 1 }
                     * Fail:    { "Status" : 0 }
                     */

                    Debug.Log("Cmd Return = " + strjson.Length.ToString());
                }

                return retstr;
            }
            catch (SocketException e)
            {
                Debug.LogError("SocketException:" + e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception:" + e.Message);
            }

            return "";
        }
    }


    public void btnControlCmd_Click(object sender, EventArgs e)
    {
        string szCmd = "{ \"CMD\" : \"Func1\" }";
        SendCommand(szCmd);
    }

    public void btnControlCmd2_Click(object sender, EventArgs e)
    {
        string szCmd = "{ \"CMD\" : \"Func2\",\r\n";
        szCmd += "\"Param1\" : 10 }";
        SendCommand(szCmd);
    }

    public void btnControlCmd3_Click(object sender, EventArgs e)
    {
        string szCmd = "{ \"CMD\" : \"SetSpeed\",\r\n";
        szCmd += "\"Param1\" : 10,\r\n";
        szCmd += "\"Param2\" : \"Video\" }";
        SendCommand(szCmd);
    }
}