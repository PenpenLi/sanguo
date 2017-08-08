using Assets.Scripts.events;
using Assets.Scripts.manager;
using com.tsixi.mars.protobuf;
using com.tsixi.miner.pbm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Scripts.net.responses
{
    class StorePropResponse : IResponse
    {
        /// <summary>
        /// 穿戴道具列表
        /// </summary>
        static public event events.MyEventArgs.MyHandler WearListHandler;
        /// <summary>
        /// 道具列表 handler
        /// </summary>
        static public event events.MyEventArgs.MyHandler PropListHandler;
        /// <summary>
        ///  购买道具结果 handler
        /// </summary>
        static public event events.MyEventArgs.MyHandler BuyResultHandler;
        // private ResModifyRoleNameResultMessage result;
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if(tmeg.cmd == 1302)//服务器返回玩家装备数据
            {
                ResWearInfo resWearInfo = NetManager.DeSerialize<ResWearInfo>(tmeg.data_message);
                PlayerManager.getInstance().resWearInfo = resWearInfo;
                Debug.Log(resWearInfo.propSids.Count);
                MyEventArgs myArgs = new MyEventArgs(resWearInfo);
                WearListHandler(this, myArgs);
            }
            else if(tmeg.cmd == 1307)//服务器返回购买结果消息
            {
                ResBuyPropResultMessage resBuyPropResulst = NetManager.DeSerialize<ResBuyPropResultMessage>(tmeg.data_message);
                Debug.Log(resBuyPropResulst.modify_result);
                MyEventArgs myArgs = new MyEventArgs(resBuyPropResulst);
                BuyResultHandler(this, myArgs);
            }
            else if (tmeg.cmd == 1304)//服务器返回指定类型道具列表
            {
                ResPropList resPropList = NetManager.DeSerialize<ResPropList>(tmeg.data_message);
                PlayerManager.getInstance().resPropList = resPropList;
                Debug.Log(resPropList.props.Count);
                MyEventArgs myArgs = new MyEventArgs();
                PropListHandler(this, myArgs);
            }
            else if (tmeg.cmd == 1308)//服务器返回穿戴装备是否成功数据
            {
                ResWearing resWearing = NetManager.DeSerialize<ResWearing>(tmeg.data_message);
                Debug.Log("resWearing.result:"+resWearing.result);
            }


            //result = NetManager.DeSerialize<ResModifyRoleNameResultMessage>((byte[])msg);
            //Debug.Log(result.modify_result);
            //MyEventArgs myArgs = new MyEventArgs(result);
            //RenameResult(this, myArgs);
        }
        
    }
}
