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
    public class ChatSocketNetworkMng : MonoBehaviour
    {
        private Dictionary<short, Action<byte[], int>> m_hadlerDic = new Dictionary<short, Action<byte[], int>>();

        
        private MyChatClient m_client;

        private void Awake()
        {
            m_client = new MyChatClient(Globals.Instance.Settings.SocketUrl,
                Globals.Instance.Settings.ChatSocketPort);

            StartCoroutine(AutoReconnect());


            
            m_hadlerDic.Add(Cmd.AudioStreamBroadcast, CmdHandler.AudioStreamBroadcast);
            
        }


        public void ForceClose()
        {
            m_client.ForceClose();
        }

        public void Handler(byte[] data)
        {
            if (data.Length < 6)
            {
                return;
            }


            //Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            //{
            //    Globals.SceneSingleton<ContextManager>().WebBlockUI(false);
            //});

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
                int dataLen = BitConverter.ToInt32(data, index + 2);

                if (foo >= 1)
                {
                    int a = 0;
                }
                
                //Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
                //{
                //    Debug.LogFormat("socket: {0},,,,{1},,,{2}", data.Length, index + 4 + dataLen + 1, dataLen);
                //};

                if(index + 6 + dataLen + 1 > data.Length)
                {
                    break;
                }

                

                short cmd = BitConverter.ToInt16(data, index);
                index += 2;
                Execute(cmd, data, index);

                index += (4 + dataLen + 1);
                if(index >= data.Length)
                {
                    break;
                }

                foo += 1;
            }

        }



        public void Execute(short cmd, byte[] data, int index)
        {
            if (!m_hadlerDic.ContainsKey(cmd))
            {
                return;
            }

            m_hadlerDic[cmd].Invoke(data, index);
        }



        //public Queue<byte[]> MessageQueue = new Queue<byte[]>();
        public void Send(byte[] data)
        {
            //MessageQueue.Enqueue(data);

            //Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            //{
            //    StartCoroutine(AutoSend(data));
            //});

            m_client.Send(data);
        }

        //IEnumerator AutoSend(byte[] data)
        //{
        //    while(MessageQueue.Count > 0)
        //    {
        //        m_client.Send(MessageQueue.Peek());

        //        yield return new WaitForSeconds(3.5f);
        //    }
        //}

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
                yield return new WaitForSeconds(5);

                if (!Globals.SceneSingleton<GameMng>().IsGaming)
                {
                    m_client.ForceClose();
                    Destroy(gameObject);
                    yield break;
                }

                if (!m_client.Connected)
                {
                    // 如果10次重连没有连接上，则退出到登录界面，将所有状态清空
                    //if(Globals.Instance.ReconnectTime < 0)
                    //{
                    //    Globals.SceneSingleton<UIManager>().DestroySingleUI(UIType.TableView);
                    //    Globals.SceneSingleton<ContextManager>().Push(new LoginUIContext());

                    //    Globals.SceneSingleton<DataMng>().ClearAll();
                    //    Globals.SceneSingleton<GameMng>().ClearAll();
                    //    Globals.SceneSingleton<SoundMng>().StopBackSound();
                    //    Globals.RemoveSceneSingleton<Osblow.Game.SocketNetworkMng>();

                    //    Globals.Instance.SaveLog();

                    //    Destroy(gameObject);
                    //    yield break;
                    //}


                    Debug.Log("正在重新连接语音服务器....");
                    Globals.SceneSingleton<ContextManager>().WebBlockUI(true, "正在连接语音服务器...");
                    m_client.ForceClose();
                    m_client = new MyChatClient(Globals.Instance.Settings.SocketUrl,
                Globals.Instance.Settings.ChatSocketPort);

                    //--Globals.Instance.ReconnectTime;
                }
                else
                {
                    //Debug.Log(Time.time);
                    //CmdRequest.ClientHeartBeatRequest();
                }
            }
        }
    }




    class MyChatClient
    {
        public bool Connected { get { return m_socket != null && m_socket.Connected; } }

        private string m_address;
        private int m_port;

        private Socket m_socket;
        private System.Threading.Thread m_socketThread;

        public MyChatClient(string addr, int port)
        {
            m_address = addr;
            m_port = port;

            //m_socketThread = new System.Threading.Thread(Connect);
            Connect();
        }

        void Connect()
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(m_address);
            IPEndPoint iep = new IPEndPoint(ip, m_port);
            Debug.Log(m_port);
            m_socket.BeginConnect(iep, new AsyncCallback(Connect), m_socket);
        }

        void Connect(IAsyncResult iar)
        {
            Socket client = (Socket)iar.AsyncState;
            try
            {
                client.EndConnect(iar);
                Receive();

                Debug.Log("已连接语音服务器");
                //MsgMng.Dispatch(MsgType.Connected);
                CmdRequest.RegistVoiceServer();
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

                //if (Globals.SceneSingleton<SocketNetworkMng>().MessageQueue.Count > 0)
                //{
                //    Globals.SceneSingleton<SocketNetworkMng>().MessageQueue.Dequeue();
                //    //Debug.Log("已发送..消息队列还剩" +
                //    //    Globals.SceneSingleton<SocketNetworkMng>().MessageQueue.Count + "条");
                //}
                Debug.LogFormat("Sent {0} bytes voice to server.", bytesSent);
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


        MyBuffer m_buffer = new MyBuffer();
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
                    Debug.Log("<color=red>不想接收 了</color>");
                    return;
                }

                // Read data from the remote device.     
                int bytesRead = client.EndReceive(ar);

                Debug.Log("get from chat server, length = " + bytesRead);
                if (bytesRead > 0)
                {

                    byte[] realData = new byte[bytesRead];
                    Array.Copy(state.Buffer, realData, bytesRead);

                    // 拼接数据包
                    if (m_buffer.TargetLength < 0)
                    {
                        m_buffer.Init(realData);

                        if (m_buffer.CheckComplete())
                        {
                            Globals.SceneSingleton<ChatSocketNetworkMng>().Handler(m_buffer.Buffer.ToArray());

                            m_buffer.Clear();
                        }
                    }
                    else
                    {
                        m_buffer.Buffer.AddRange(realData);

                        if (m_buffer.CheckComplete())
                        {
                            Globals.SceneSingleton<ChatSocketNetworkMng>().Handler(m_buffer.Buffer.ToArray());

                            m_buffer.Clear();
                        }
                    }
                    Receive();
                    
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
            //m_socketThread.Abort();
        }

        public void ForceClose()
        {
            m_socket.Close();
            //m_socketThread.Abort();
        }



        class MyBuffer
        {
            public List<byte> Buffer = new List<byte>();
            public int TargetLength = -1;

            
            public void Init(byte[] data)
            {
                TargetLength = BitConverter.ToInt32(data, 3);
                Buffer.AddRange(data);
            }

            public void Clear()
            {
                Buffer.Clear();
                TargetLength = -1;
            }

            /// <summary>
            /// 检查数据包完整性
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            public bool CheckComplete()
            {
                Debug.LogFormat("targetLength:{0}, standardSize:{1}, realSize:{2}", TargetLength, 7 + TargetLength + 1, Buffer.Count);
                if(TargetLength > 0 && 7 + TargetLength + 1 > Buffer.Count)
                {
                    return false;
                }

                return true;
            }
        }
    }
    
}
