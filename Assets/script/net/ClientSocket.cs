using Assets.Scripts.manager;
using Assets.Scripts.tool;
using com.tsixi.mars.protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.net {
    class ClientSocket {
        private const int headlen = 4;
        //每次接受和发送数据的大小
        private const int size = 1024;
        private const int pingpong = 30 * 1000;
        public Socket socket;
        public byte[] head = new byte[headlen];
        //发送数据池
        private Queue<byte[]> sendCache = new Queue<byte[]>();
        private bool isSending;

        private System.Timers.Timer timer;
        public long serverTimeOffset = 0;

        public MessageDispatcher messageDispatcher;

        public ClientSocket(string ips, int port) {
            Debug.LogFormat("开始创建连接，ip={0},port={1}", ips, port);
            //实例化Socket对象
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ips);//服务器ip地址
            IPEndPoint ipe = new IPEndPoint(ip, port);//服务器端口
            IAsyncResult result = socket.BeginConnect(ipe, new AsyncCallback(ConnectCallBack), socket);
            bool connectsucces = result.AsyncWaitHandle.WaitOne(5000, true);//超时检测
            if (connectsucces) {
                Debug.Log("bigen recive");
            } else {
                Debug.Log("Time Out");
            }
        }
        /// <summary>
        /// 成功建立连接回调方法
        /// </summary>
        /// <param name="ast"></param>
        private void ConnectCallBack(IAsyncResult ast) {
            socket.EndConnect(ast);
            IPEndPoint ipEndPoint = socket.RemoteEndPoint as IPEndPoint;
            Debug.LogFormat("Connect Success ip={0},port={1}", ipEndPoint.Address, ipEndPoint.Port);
            socket.BeginReceive(head, 0, headlen, SocketFlags.None, new AsyncCallback(ReceiveHead), 0);
            SyncServerTime();
        }

        private void ReceiveHead(IAsyncResult ar) {
            int count = (int)ar.AsyncState;
            count += socket.EndReceive(ar);
            //如果头数据长度不够，继续读
            if (count < headlen) {
                socket.BeginReceive(head, count, headlen - count, SocketFlags.None, new AsyncCallback(ReceiveHead), count);
            } else {
                try {
                    //处理一下高低字节位问题
                    byte[] realBytesLen = new byte[headlen];
                    for (int i = 0; i < headlen; i++) {
                        realBytesLen[i] = head[headlen - i];
                    }
                    int length = ByteUtil.byteArray2Int(realBytesLen, 0);
                    byte[] data = new byte[length];//声明接受数组
                    count = 0;
                    while (count < length) {
                        int tempLength = socket.Receive(data, count, length - count, SocketFlags.None);
                        count += tempLength;
                    }
                    socket.BeginReceive(head, 0, headlen, SocketFlags.None, new AsyncCallback(ReceiveHead), 0);
                    MarsMessage msg = ProtobufTool.DeSerialize<MarsMessage>(data);
                    messageDispatcher.Receive(msg);
                } catch (Exception e) {
                    Debug.Log(e.ToString());
                }
            }
        }

        // 服务器发送消息给客户端
        public void Send() {
            try {
                if (sendCache.Count == 0) {
                    isSending = false;
                    return;
                }
                byte[] data = sendCache.Dequeue();
                socket.Send(data, 0, data.Length, SocketFlags.None);
                Debug.Log("发送成功!");
                Send();
            } catch (Exception ex) {
                Debug.Log(ex.ToString());
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendMeg"></param>
        /// <param name="message_type"></param>
        /// <param name="cmd"></param>
        public void WriteSend(object sendMeg) {
            //  Debug.Log(sendMeg.GetType().GetFields());
            //Debug.Log(sendMeg.GetType().GetMethod("get_cmd").Invoke(sendMeg,null));
            //Debug.Log(sendMeg.GetType().GetField("_cmd").GetValue(sendMeg));
            if (!socket.Connected)//断开连接
            {
                Debug.Log("断开连接!!!!!");
                socket.Close();
                StopPing();
                return;
            }

            uint sendMessageType = (uint)sendMeg.GetType().GetMethod("get_message_type").Invoke(sendMeg, null);
            uint sendCmd;
            TXMessage tXMessage = new TXMessage();
            if (sendMessageType <= 10) {//ping 消息

            } else {
                sendCmd = (uint)sendMeg.GetType().GetMethod("get_cmd").Invoke(sendMeg, null);
                tXMessage.cmd = sendCmd;
            }

            tXMessage.message_type = sendMessageType;
            tXMessage.data_message = ProtobufTool.Serialize(sendMeg);
            WriteSendDate(tXMessage);

        }
        private void WriteSendDate(TXMessage sendMeg) {
            byte[] msBts = ProtobufTool.Serialize(sendMeg);
            byte[] result = new byte[msBts.Length + 4];
            byte[] lenghtBytes = ByteUtil.int2ByteArray(msBts.Length);
            if (sendMeg.message_type != 1)//!ping
                Debug.Log("开始发送msBts.Length" + msBts.Length + "..." + ((TXMessage)sendMeg).message_type + ":" + ((TXMessage)sendMeg).cmd);
            for (int i = 0; i < 4; i++) {
                result[i] = lenghtBytes[3 - i];
            }
            for (int i = 0; i < msBts.Length; i++) {
                result[i + 4] = msBts[i];
            }

            sendCache.Enqueue(result);
            if (!isSending) {
                isSending = true;
                Send();
            }
        }

        // 将数据编码 长度+内容
        /// < param name="data">内容< /param>
        public static byte[] Encode(byte[] data) {
            //整形占四个字节，所以声明一个+4的数组
            byte[] result = new byte[data.Length + 4];
            //使用流将编码写二进制
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(data.Length);
            br.Write(data);
            //将流中的内容复制到数组中
            System.Buffer.BlockCopy(ms.ToArray(), 0, result, 0, (int)ms.Length);
            br.Close();
            ms.Close();
            return result;
        }

        /// <summary>
        /// 同步服务器时间
        /// </summary>
        public void SyncServerTime() {
        }

        /// <summary>
        /// 时间同步返回
        /// </summary>
        public void OnSyncServerTime(int serverTimeOffset) {
            this.serverTimeOffset = serverTimeOffset;
            StartPing();
        }

        /// <summary>
        /// 启动心跳发送
        /// </summary>
        public void StartPing() {
            timer = new System.Timers.Timer(pingpong);   //实例化Timer类，设置间隔时间为10000毫秒；   
            timer.Elapsed += new System.Timers.ElapsedEventHandler(PingPong); //到达时间的时候执行事件；   
            timer.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            timer.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

        }
        /// <summary>
        /// 取消心跳发送
        /// </summary>
        public void StopPing() {
            if (timer != null) {
                timer.Enabled = false;
                timer.Dispose();
            }

        }
        /// <summary>
        /// 发送心跳消息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void PingPong(object source, System.Timers.ElapsedEventArgs e) {
            ReqPingMessage reqPing = new ReqPingMessage() {
                time = (ulong)(Tool.ToGMTTime(DateTime.Now) + serverTimeOffset)
            };
            WriteSend(reqPing);
        }

        /// <summary>
        /// 关闭当前连接
        /// </summary>
        public void Close() {
            StopPing();
            socket.Close(0);
        }
    }
}
