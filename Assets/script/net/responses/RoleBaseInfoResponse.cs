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
    class RoleBaseInfoResponse : IResponse
    {
        /// <summary>
        /// 角色当前货币信息 handler
        /// </summary>
       // static public event events.MyEventArgs.MyHandler GoldInfoHandler;
        static public string EVENT_UPDATA_CURRENCY = "EVENT_UPDATA_CURRENCY";
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if (tmeg.cmd == 1009)
            {
                ResCurrency resCurrency = NetManager.DeSerialize<ResCurrency>(tmeg.data_message);
                PlayerManager.getInstance().RoleInfo.gold = resCurrency.golds;
                PlayerManager.getInstance().RoleInfo.diamond = resCurrency.diamonds;
                Debug.Log("goldupdata");
                EventDispatcher.Instance().DispatchEvent(EVENT_UPDATA_CURRENCY, resCurrency);
                //MyEventArgs myArgs = new MyEventArgs(resCurrency);
                //GoldInfoHandler(this, myArgs);
            }

            //result = NetManager.DeSerialize<ResModifyRoleNameResultMessage>((byte[])msg);
            //Debug.Log(result.modify_result);
            //MyEventArgs myArgs = new MyEventArgs(result);
            //RenameResult(this, myArgs);
        }

    }
}
