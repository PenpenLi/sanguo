using Assets.Scripts.manager;
using com.tsixi.mars.protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.net
{
    class ClientSocket
    {
        public Socket mySocket;

        //连接客户端的Socket
        //public Socket socket;
        //用于存放接收数据
        public byte[] buffer;
        //每次接受和发送数据的大小
        private const int size = 1024;

        //接收数据池
        private List<byte> receiveCache = new List<byte>();
        private bool isReceiving;
        //发送数据池
        private Queue<byte[]> sendCache = new Queue<byte[]>();
        private bool isSending;

        //接收到消息之后的回调
        // public Action<NetModel> receiveCallBack;

        
        public ClientSocket(string ips,int port)
        {
            //实例化Socket对象
            mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ip = IPAddress.Parse("172.19.37.5");//倪文帅Wifi
            //IPAddress ip = IPAddress.Parse("192.168.1.109");//服务器ip地址  7437
            IPAddress ip = IPAddress.Parse(ips);//服务器ip地址  工作室
            IPEndPoint ipe = new IPEndPoint(ip, port);//服务器端口
            IAsyncResult result = mySocket.BeginConnect(ipe, new AsyncCallback(connectCallBack), mySocket);
            
            bool connectsucces = result.AsyncWaitHandle.WaitOne(5000, true);//超时检测

            if (connectsucces)//连接成功
            {
                Debug.Log("bigen recive");
                Thread thread = new Thread(new ThreadStart(getMSG));//从服务器接受消息
                //thread.IsBackground = true;
                thread.Start();
            }
            else//没有连接成功
            {
                Debug.Log("Time Out");
            }
        }
        private void connectCallBack(IAsyncResult ast)//成功建立连接回调方法
        {
            Debug.Log("Connect Success");
            //  NetManager.getIntance().synServerTime();
            NetManager.getIntance().startPing();
        }
        private void getMSG()
        {
            Debug.Log("接收到数据");
            while (true)
            {
               // Debug.Log("getMSG true");
                if (!mySocket.Connected)//断开连接
                {
                    Debug.Log("bread connect");
                    mySocket.Close();
                    break;
                }
                try
                {
                    //将接收到的数据放入数据池中
                    byte[] bytesLen = new byte[4];
                    mySocket.Receive(bytesLen);//接受长度

                    byte[] realBytesLen = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        realBytesLen[i] = bytesLen[3 - i];
                    }
                    int length = ByteUtil.byteArray2Int(realBytesLen, 0);
                   // Debug.Log("buf length:"+length);

                    byte[] byteData = new byte[length];//声明接受数组
                    int count = 0;
                    while (count < length)
                    {
                        int tempLength = mySocket.Receive(byteData, count, length - count, SocketFlags.None);
                      //  Debug.Log("buf tempLength:" + tempLength);
                        count += tempLength;
                      //  Debug.Log("buf count:" + count);
                    }
                     
                    byte[] bytes = new byte[length + 4];//声明接受数组
                    Array.Copy(realBytesLen, 0, bytes, 0, 4);
                    Array.Copy(byteData, 0, bytes, 4, length);

                    //byte[] bytesAll = new byte[1024];
                    //int len = mySocket.Receive(bytesAll);
                    //byte[] bytes = new byte[len];//声明接受数组
                    //Array.Copy(bytesAll,bytes, len);


                    receiveCache.AddRange(bytes);
                    //如果没在读数据
                    if (!isReceiving)
                    {
                        isReceiving = true;
                        ReadData();
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    break;
                }
            }
        }

        // 读取数据
        private void ReadData(int length = 0)
        {
            byte[] data = Decode(ref receiveCache);
            TXMessage result = null;
           // Debug.Log("buf ReadData");
            //说明数据保存成功
            if (data != null)
            {
            //    Debug.Log("buf data.lenght:"+data.Length);
                // NetModel item = NetSerilizer.DeSerialize(data);
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //将消息写入流中
                        ms.Write(data, 0, data.Length);
                        //将流的位置归0
                        ms.Position = 0;
                        //使用工具反序列化对象
                        result = ProtoBuf.Serializer.Deserialize<TXMessage>(ms);
                        if(result.message_type !=1)//ping 不打印
                           Debug.Log(result.message_type+":"+result.cmd);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("反序列化失败: " + ex.ToString());
                }
                NetManager.getIntance().handle(result);
                // UnityEngine.Debug.Log(result.Message);
                //if (receiveCallBack != null)
                //{
                //    receiveCallBack(item);
                //}
                //尾递归，继续读取数据
                ReadData();
            }
            else
            {
                isReceiving = false;
            }
        }

        // 服务器发送消息给客户端
        public void Send()
        {
           // UnityEngine.Debug.Log("send");
            try
            {
                if (sendCache.Count == 0)
                {
                    isSending = false;
                    return;
                }
                byte[] data = sendCache.Dequeue();
                //int count = data.Length / size;
                //int len = size;
                //for (int i = 0; i < count + 1; i++)
                //{
                //    if (i == count)
                //    {
                //        len = data.Length - i * size;
                //    }
                //    mySocket.Send(data, i * size, len, SocketFlags.None);
                //}
                mySocket.Send(data, 0, data.Length, SocketFlags.None);
                UnityEngine.Debug.Log("发送成功!");
                Send();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.ToString());
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendMeg"></param>
        /// <param name="message_type"></param>
        /// <param name="cmd"></param>
        public void WriteSend(object sendMeg )  
        {
            //  Debug.Log(sendMeg.GetType().GetFields());
            //Debug.Log(sendMeg.GetType().GetMethod("get_cmd").Invoke(sendMeg,null));
            //Debug.Log(sendMeg.GetType().GetField("_cmd").GetValue(sendMeg));
            if (!mySocket.Connected)//断开连接
            {
                Debug.Log("断开连接!!!!!");
                mySocket.Close();
                NetManager.getIntance().stopPing();
                return;
            }

            uint sendMessageType = (uint)sendMeg.GetType().GetMethod("get_message_type").Invoke(sendMeg, null);
            uint sendCmd;
            TXMessage tXMessage = new TXMessage();
            if (sendMessageType <= 10) {//ping 消息

            }
            else
            {
                sendCmd = (uint)sendMeg.GetType().GetMethod("get_cmd").Invoke(sendMeg, null);
                tXMessage.cmd = sendCmd;
            }
           
            tXMessage.message_type = sendMessageType;
            tXMessage.data_message = NetManager.Serialize(sendMeg);
            WriteSendDate(tXMessage);

        }
        private void WriteSendDate(TXMessage sendMeg)
        {
            
            byte[] msBts = NetManager.Serialize(sendMeg);
            byte[] result = new byte[msBts.Length + 4];
            byte[] lenghtBytes = ByteUtil.int2ByteArray(msBts.Length);
            if(sendMeg.message_type != 1)//!ping
                 Debug.Log("开始发送msBts.Length" + msBts.Length+"..."+ ((TXMessage)sendMeg).message_type+":"+((TXMessage)sendMeg).cmd);
            for (int i = 0; i < 4; i++)
            {
                result[i] = lenghtBytes[3 - i];
            }
            for (int i = 0; i < msBts.Length; i++)
            {
                result[i + 4] = msBts[i];
            }
           
            sendCache.Enqueue(result);
            if (!isSending)
            {
                isSending = true;
                Send();
            }
        }

        //public void sendMSG(byte[] bytes)
        //{
        //    if (!mySocket.Connected)//断开连接
        //    {
        //        Debug.Log("bread connect");
        //        mySocket.Close();
        //    }
        //    try
        //    {
        //        int length = bytes.Length;
        //        byte[] blength = ByteUtil.int2ByteArray(length);
        //        mySocket.Send(blength, SocketFlags.None);//发长度
        //        mySocket.Send(bytes, SocketFlags.None);//发数据

        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Log(e.ToString());
        //    }
        //}


        // 将数据编码 长度+内容
        /// < param name="data">内容< /param>
        public static byte[] Encode(byte[] data)
        {
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

        // 将数据解码
        // < param name="cache">消息队列< /param>
        public static byte[] Decode(ref List<byte> cache)
        {
            //首先要获取长度，整形4个字节，如果字节数不足4个字节
            if (cache.Count < 4)
            {
                return null;
            }
            //读取数据
            //byte[] byteOri = cache.ToArray();
            //byte[] byteDes = new Byte[byteOri.Length];
            //for(int i= 0;i< byteOri.Length; i++)
            //{
            //    if (i < 4)//长度 交换
            //    {
            //        byteDes[i] = byteOri[3 - i];
            //    }else
            //    {
            //        byteDes[i] = byteOri[i];
            //    }
            //}
           // MemoryStream ms = new MemoryStream(byteDes);

            MemoryStream ms = new MemoryStream(cache.ToArray());
            BinaryReader br = new BinaryReader(ms);
            int len = br.ReadInt32();
            //根据长度，判断内容是否传递完毕
            if (len > ms.Length - ms.Position)
            {
                return null;
            }
            //获取数据
            byte[] result = br.ReadBytes(len);
            //清空消息池
            cache.Clear();
            //讲剩余没处理的消息存入消息池
            cache.AddRange(br.ReadBytes((int)ms.Length - (int)ms.Position));

            return result;
        }
    }


    class sendObj

    {
        public uint cmd { get; set; }
        

    }
}
