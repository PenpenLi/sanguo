using Assets.Scripts.events;
using Assets.Scripts.manager;
using Assets.Scripts.tool;
using com.tsixi.mars.protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.net.responses
{
    class ServerTimeResponse : IResponse
    {
       // static public event events.MyEventArgs.MyHandler RoleInfoHandler ;

        private ResServerTime result;
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if (tmeg.cmd == 0)//玩家的所有任务
            {
                result = NetManager.DeSerialize<ResServerTime>(tmeg.data_message);
                
                TimeSpan timeSpan = DateTime.Now - NetManager.getIntance().beginTime;
                long ping = (long)timeSpan.TotalMilliseconds / 2;
                long serverTime = (long)result.time + ping;
                long serverTimeOffset = serverTime - Tool.ToGMTTime(DateTime.Now);
                NetManager.getIntance().serverTimeOffset = serverTimeOffset;
                NetManager.getIntance().startPing();
                Debug.Log("ping:" + ping + ",serverTimeOffset:" + serverTimeOffset);
            }


           
        }
    }
}
