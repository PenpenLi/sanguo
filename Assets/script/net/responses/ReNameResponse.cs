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
    class ReNameResponse : IResponse
    {
        static public event events.MyEventArgs.MyHandler RenameResult;
        private ResModifyRoleNameResultMessage result;
        public void handler(object msg)
        {
            result = NetManager.DeSerialize<ResModifyRoleNameResultMessage>((byte[])msg);
            Debug.Log(result.modify_result);
            //MyEventArgs myArgs = new MyEventArgs(result);
            //RenameResult(this, myArgs);

            EventDispatcher.Instance().DispatchEvent("ReName", result);
        }
    }
}
