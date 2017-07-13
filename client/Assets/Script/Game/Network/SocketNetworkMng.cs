using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using Osblow.App;
using Osblow.Util;


namespace Osblow.Game
{
    public class SocketNetworkMng : MonoBehaviour
    {
        private Dictionary<short, Action<byte[], int>> m_hadlerDic = new Dictionary<short, Action<byte[], int>>();

        
        private MyClient m_client;

        private void Awake()
        {
            m_client = new MyClient(Globals.Instance.Settings.SocketUrl, 
                Globals.Instance.Settings.SocketPort);

            StartCoroutine(AutoReconnect());



            m_hadlerDic.Add(Cmd.ServerRegisterResponse, CmdHandler.ServerRegisterResponse);
            m_hadlerDic.Add(Cmd.ServerHeartbeatResponse, CmdHandler.ServerHeartbeatResponse);
            m_hadlerDic.Add(Cmd.EnterRoomResponse, CmdHandler.EnterRoomResponse);
            m_hadlerDic.Add(Cmd.EnterRoomOtherResponse, CmdHandler.EnterRoomOtherResponse);
            m_hadlerDic.Add(Cmd.ExitRoomToOtherResponse, CmdHandler.ExitRoomToOtherResponse);
            m_hadlerDic.Add(Cmd.ExitRoomResultResponse, CmdHandler.ExitRoomResultResponse);
            m_hadlerDic.Add(Cmd.DismissRoomToOtherResponse, CmdHandler.DismissRoomToOtherResponse);
            m_hadlerDic.Add(Cmd.PlayerVoteDismissRoomResponse, CmdHandler.PlayerVoteDismissRoomResponse);
            m_hadlerDic.Add(Cmd.DismissRoomResultResponse, CmdHandler.DismissRoomResultResponse);
            m_hadlerDic.Add(Cmd.ReadyResponse, CmdHandler.ReadyResponse);
            m_hadlerDic.Add(Cmd.ReconnectResponse, CmdHandler.ReconnectResponse);
            m_hadlerDic.Add(Cmd.OnlineStatusResponse, CmdHandler.OnlineStatusResponse);
            m_hadlerDic.Add(Cmd.SynchroniseExpressionResponse, CmdHandler.SynchroniseExpressionResponse);
            m_hadlerDic.Add(Cmd.AudioStreamBroadcast, CmdHandler.AudioStreamBroadcast);
            m_hadlerDic.Add(Cmd.ServerBetResponse, CmdHandler.ServerBetResponse);
            m_hadlerDic.Add(Cmd.ServerBankerNotice1Response, CmdHandler.ServerBankerNotice1Response);
            m_hadlerDic.Add(Cmd.ServerBankerNotice2Response, CmdHandler.ServerBankerNotice2Response);
            m_hadlerDic.Add(Cmd.ServerBetAgainResponse, CmdHandler.ServerBetAgainResponse);
            m_hadlerDic.Add(Cmd.ServerBankerResponse, CmdHandler.ServerBankerResponse);
            m_hadlerDic.Add(Cmd.ServerCardsResponse, CmdHandler.ServerCardsResponse);
            m_hadlerDic.Add(Cmd.SynchroniseCardsResponse, CmdHandler.SynchroniseCardsResponse);
            m_hadlerDic.Add(Cmd.WinOrLoseResponse, CmdHandler.WinOrLoseResponse);
            m_hadlerDic.Add(Cmd.UserAllResult, CmdHandler.UserAllResult);
            /*
             * public static short StartGameResponse = 0x1037;
        public static short ServerBetOverResponse = 0x1038;
        public static short ServerToBankerResponse = 0x1039;
             */
            m_hadlerDic.Add(Cmd.StartGameResponse, CmdHandler.StartGameResponse);
            m_hadlerDic.Add(Cmd.ServerBetOverResponse, CmdHandler.ServerBetOverResponse);
            m_hadlerDic.Add(Cmd.ServerToBankerResponse, CmdHandler.ServerToBankerResponse);
        }

        public void Handler(byte[] data)
        {
            if (data.Length < 6)
            {
                return;
            }


            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                Globals.SceneSingleton<ContextManager>().WebBlockUI(false);
            };

            int foo = 0;

            int index = 0;
            // 处理粘包
            while (true)
            {
                byte flag = data[index];
                if (flag != 0x64)
                {
                    return;
                }
                index += 1;
                short dataLen = BitConverter.ToInt16(data, index + 2);

                if (foo >= 1)
                {
                    int a = 0;
                }
                //Debug.LogFormat("socket: {0},,,,{1},,,{2}", data.Length, index + 4 + dataLen + 1, dataLen);
                if(index + 4 + dataLen + 1 > data.Length)
                {
                    break;
                }

                

                short cmd = BitConverter.ToInt16(data, index);
                index += 2;
                Execute(cmd, data, index);

                index += (2 + dataLen + 1);
                if(index >= data.Length)
                {
                    break;
                }

                foo += 1;
            }

        }



        public void Execute(short cmd, byte[] data, int index)
        {
            if(cmd != 0x1004)
            {
                int a = 0;
            }

            if (!m_hadlerDic.ContainsKey(cmd))
            {
                return;
            }

            m_hadlerDic[cmd].Invoke(data, index);
        }



        public void Send(byte[] data)
        {
            m_client.Send(data);
        }

        private void OnDestroy()
        {
            if (!m_client.Connected)
            {
                return;
            }

            m_client.Close();
        }

        private IEnumerator AutoReconnect()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);

                if (!Globals.SceneSingleton<GameMng>().IsGaming)
                {
                    Destroy(gameObject);
                    yield break;
                }

                if (!m_client.Connected)
                {
                    Debug.Log("正在重新连接....");
                    Globals.SceneSingleton<ContextManager>().WebBlockUI(true);
                    m_client.ForceClose();
                    m_client = new MyClient(Globals.Instance.Settings.SocketUrl,
                Globals.Instance.Settings.SocketPort);
                }
                else
                {
                    //Debug.Log(Time.time);
                    CmdRequest.ClientHeartBeatRequest();
                }
            }
        }
    }




    class MyClient
    {
        public bool Connected { get { return m_socket != null && m_socket.Connected; } }

        private Socket m_socket;


        public MyClient(string addr, int port)
        {
            Connect(addr, port);
        }

        void Connect(string addr, int port)
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(addr);
            IPEndPoint iep = new IPEndPoint(ip, port);
            m_socket.BeginConnect(iep, new AsyncCallback(Connect), m_socket);
        }

        void Connect(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            try
            {
                client.EndConnect(iar);
                Receive();

                Debug.Log("已连接");
                MsgMng.Dispatch(MsgType.Connected);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {

            }
        }

        public void Send(byte[] data)
        {
            if (!m_socket.Connected)
            {
                return;
            }

            // Begin sending the data to the remote device.     
            m_socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), m_socket);
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.     
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.     
                int bytesSent = handler.EndSend(ar);
                //Debug.LogFormat("Sent {0} bytes to server.", bytesSent);
                //NetworkMng.Instance.DebugStr = string.Format("Sent {0} bytes to server.", bytesSent);
            }
            catch (SocketException e)
            {
                Debug.Log(e.ToString());
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        private void Receive()
        {
            try
            {
                // Create the state object.     
                TCPState state = new TCPState(m_socket);

                // Begin receiving the data from the remote device.     
                m_socket.BeginReceive(state.Buffer, 0, TCPState.BuffSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket     
                // from the asynchronous state object.     
                TCPState state = (TCPState)ar.AsyncState;
                Socket client = state.Socket;

                if(client == null || !client.Connected)
                {
                    return;
                }

                // Read data from the remote device.     
                int bytesRead = client.EndReceive(ar);

                //Debug.Log("get from server, length = " + bytesRead);
                if (bytesRead > 0)
                {
                    byte[] realData = new byte[bytesRead];
                    Array.Copy(state.Buffer, realData, bytesRead);
                    Globals.SceneSingleton<SocketNetworkMng>().Handler(realData);

                    // Get new data.     
                    client.BeginReceive(state.Buffer, 0, TCPState.BuffSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (SocketException e)
            {
                Debug.Log(e.ToString());
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public void Close()
        {
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
        }

        public void ForceClose()
        {
            m_socket.Close();
        }
    }

    public class TCPState
    {
        public const int BuffSize = 4096;
        public byte[] Buffer = new byte[4096];

        private Socket socket = null;

        public Socket Socket
        {
            get { return socket; }
        }

        public TCPState(Socket socket)
        {
            this.socket = socket;
        }
    }
}
