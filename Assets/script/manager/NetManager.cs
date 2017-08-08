using Assets.Scripts.net;
using Assets.Scripts.net.responses;
using Assets.Scripts.tool;
using com.tsixi.mars.protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.manager
{
    class NetManager
    {
        /// <summary>
        /// game socket
        /// </summary>
        static public ClientSocket clientSocket;

        static public Dictionary<uint ,IResponse> ResponseDic = new Dictionary<uint, IResponse>();
        public NetManager()
        {
           
        }

        static private NetManager _instance;
        static public NetManager getIntance()
        {
            if (_instance == null)
            {
                _instance = new NetManager();
            }
            return _instance;
        }
        public void initDic()
        {
            ResponseDic.Add(NetConst.PING_S, new PingResponse());
            ResponseDic.Add(NetConst.SERVER_TIME_S, new ServerTimeResponse());
            ResponseDic.Add(NetConst.LOGIN_S, new LoginResponse());
            ResponseDic.Add(NetConst.CREAT_ROLE_S, new CreatRoleResponse());
            ResponseDic.Add(NetConst.ROLE_INFO_S, new RoleInfoResponse());
            ResponseDic.Add(NetConst.UP_BATTLE_DATA_S, new UpBattleDataResponse());
            ResponseDic.Add(NetConst.SROCE_RANK_LIST, new ScoreRankListResponse());
            ResponseDic.Add(NetConst.RENAME_S, new ReNameResponse());

            ResponseDic.Add(NetConst.ROLE_BASE_INFO_S, new RoleBaseInfoResponse());
            ResponseDic.Add(NetConst.STORE_PROP_S, new StorePropResponse());
            ResponseDic.Add(NetConst.TASK_S, new TaskResponse());
            ResponseDic.Add(NetConst.QUEST_S, new QuestResponse());
            ResponseDic.Add(NetConst.AWARD_S, new AwardResponse());
            ResponseDic.Add(NetConst.BATTLE_S, new BattleResponse());
            ResponseDic.Add(NetConst.SEASON_S, new SeasonResponse());
        }

        public void handle(TXMessage tmeg)
        {
            if (ResponseDic.Count <= 0)
            {
                initDic();
            }

            if (tmeg.message_type == 1 && tmeg.cmd == NetConst.PING_S)//ping 
            {
                IResponse response = ResponseDic[tmeg.cmd];
                response.handler(tmeg.data_message);
            }else if( (tmeg.message_type == 1000 && tmeg.cmd != 1009)  || tmeg.message_type == 1200)
            {
                IResponse response = ResponseDic[tmeg.cmd];
                if (response == null)
                {
                    Debug.Log("cmd::" + tmeg.cmd + " is not in dictionary");
                }
                else
                {
                    response.handler(tmeg.data_message);
                }
            }else
            {
                if (ResponseDic.ContainsKey(tmeg.message_type)){
                    IResponse response = ResponseDic[tmeg.message_type];
                    response.handler(tmeg);
                }else{
                    Debug.Log("message_type::" + tmeg.message_type + " is not in dictionary");
                }
            }
        }


        public DateTime beginTime;
        public void synServerTime()
        {
            Debug.Log("syn server time.");
            ReqServerTime reqServerTime = new ReqServerTime();
            beginTime = DateTime.Now;
            NetManager.clientSocket.WriteSend(reqServerTime);

        }

        System.Timers.Timer t;

        public long serverTimeOffset = 0;
        /// <summary>
        /// 
        /// </summary>
        public void startPing()
        {
            t = new System.Timers.Timer(25 * 1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //到达时间的时候执行事件；   
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
            
        }
        public void stopPing()
        {
            if(t != null)
            {
                t.Enabled = false;
                t.Dispose();
            }
           
        }
        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            
            //DateTime d1 = System.DateTime.Now;
            //DateTime d2 = new DateTime(1970, 1, 1);
            //double t = DateTime.Now.Subtract(d2).TotalMilliseconds;
            //Debug.Log("t.ToString():" + t.ToString());
            //Debug.Log("ToLongTimeString" + DateTime.Now.ToLongTimeString());
            //Debug.Log("ToBinary:" + DateTime.Now.ToBinary());
            //Debug.Log("Ticks:" + DateTime.Now.Ticks);

            //ReqPingMessage reqPing = new ReqPingMessage();
            //reqPing.time = ulong.Parse(t.ToString())  ;
            //Debug.Log(reqPing.time);

            //NetManager.clientSocket.WriteSend(reqPing);

            //DateTime time197011 = new DateTime(1970, 1, 1);
            //DateTime time = DateTime.Now;
            //TimeSpan ts = time - time197011;
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            //TimeSpan off = localZone.GetUtcOffset(time);
            //ts -= off;

            ReqPingMessage reqPing = new ReqPingMessage();
           // Debug.Log(ts.TotalSeconds);
            reqPing.time = (ulong)(Tool.toGMTTime(DateTime.Now)+serverTimeOffset);
            NetManager.clientSocket.WriteSend(reqPing);
        }

        /// <summary>
        /// 
        /// </summary>
        public void stopNet()
        {
            Debug.Log("stopNet timer");
            stopPing();
            if (NetManager.clientSocket != null)
            {
                NetManager.clientSocket.mySocket.Close();
            }
                
        }





        /// <summary>
        /// 将消息序列化为二进制的方法
        /// </summary>
        /// <param name="model">要序列化的对象</param>
        /// <returns></returns>
        static public byte[] Serialize(object model)
        {
            try
            {
                //涉及格式转换，需要用到流，将二进制序列化到流中
                using (MemoryStream ms = new MemoryStream())
                {
                    //使用ProtoBuf工具的序列化方法
                    ProtoBuf.Serializer.Serialize(ms, model);
                    //定义二级制数组，保存序列化后的结果
                    byte[] result = new byte[ms.Length];
                    //将流的位置设为0，起始点
                    ms.Position = 0;
                    //将流中的内容读取到二进制数组中
                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("序列化失败: " + ex.ToString());
                return null;
            }
        }


        // 将收到的消息反序列化成对象
        // < returns>The serialize.< /returns>
        // < param name="msg">收到的消息.</param>
       static public T DeSerialize<T>(byte[] msg)
        {
            T result ;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //将消息写入流中
                    ms.Write(msg, 0, msg.Length);
                    //将流的位置归0
                    ms.Position = 0;
                    //使用工具反序列化对象
                    result = ProtoBuf.Serializer.Deserialize<T>(ms);
                    ms.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("反序列化失败: " + ex.ToString());
                return default(T);
            }
        }
    }
}
