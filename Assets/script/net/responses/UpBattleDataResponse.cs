using Assets.Scripts.events;
using Assets.Scripts.manager;
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
    class UpBattleDataResponse : IResponse
    {
        static public event events.MyEventArgs.MyHandler UpBattleDataHandler;

        private ResUpBattleDataMessage result;
        public void handler(object msg)
        {

            result = NetManager.DeSerialize<ResUpBattleDataMessage>((byte[])msg);

            //MyEventArgs myArgs = new MyEventArgs(result);
            //RoleInfoHandler(this, myArgs);
            Debug.Log("ResUpBattleDataMessage:"+result.up_result);
        }
    }
}
