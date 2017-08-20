using Assets.Scripts.tool;
using org.alan.chess.proto;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.Scripts.net {
    class ClientSocket {
        private const int headlen = 4;
        //每次接受和发送数据的大小
        private const int size = 1024;
        private const int pingInterval = 30 * 1000;
        public Socket socket;
        public byte[] head = new byte[headlen];
        //发送数据池
        private Queue<byte[]> sendCache = new Queue<byte[]>();
        private bool isSending;

        private System.Timers.Timer timer;

        public long lastPingTime = 0;
        //消息延时值
        public int ping;
        //与服务器的时间差
        public long serverTimeOffset = 0;

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
            StartPing();
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
                    //心跳消息直接在本类处理
                    if (msg.messageType == 1) {
                        Pong pong = ProtobufTool.DeSerialize<Pong>(msg.data);
                        Pong(pong);
                    } else {
                        MessageDispatcher.Receive(msg);
                    }
                } catch (Exception e) {
                    Debug.Log(e.ToString());
                }
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendMeg"></param>
        public void Send(MarsMessage marsMessage) {
            WriteSendDate(marsMessage);

        }
        private void WriteSendDate(MarsMessage marsMessage) {
            byte[] msBts = ProtobufTool.Serialize(marsMessage);
            byte[] data = new byte[msBts.Length + 4];
            byte[] lenghtBytes = ByteUtil.int2ByteArray(msBts.Length);
            if (marsMessage.messageType != 1)//!ping
                Debug.Log("开始发送msBts.Length" + msBts.Length + "..." + marsMessage.messageType + ":" + marsMessage.cmd);
            for (int i = 0; i < 4; i++) {
                data[i] = lenghtBytes[3 - i];
            }
            for (int i = 0; i < msBts.Length; i++) {
                data[i + 4] = msBts[i];
            }
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, callback => {
                socket.EndSend(callback);
                //Debug.Log("消息发送成功");
            }, socket);
        }

        /// <summary>
        /// 启动心跳发送
        /// </summary>
        public void StartPing() {
            timer = new System.Timers.Timer(pingInterval);   //实例化Timer类，设置间隔时间为10000毫秒；   
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Ping); //到达时间的时候执行事件；   
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
        private void Ping(object source, System.Timers.ElapsedEventArgs e) {
            lastPingTime = Tool.ToGMTTime(DateTime.Now);
            MarsMessage ping = new MarsMessage() {
                messageType = 1,
                cmd = 1
            };
            Send(ping);
        }

        /// <summary>
        /// 服务器心跳响应
        /// </summary>
        /// <param name="pong"></param>
        private void Pong(Pong pong) {
            long currentTime = Tool.ToGMTTime(DateTime.Now);
            ping = (int)(currentTime - lastPingTime) / 2;
            serverTimeOffset = ping + (pong.time - lastPingTime);
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
