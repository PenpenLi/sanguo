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
    class RoleInfoResponse : IResponse
    {
        static public event events.MyEventArgs.MyHandler RoleInfoHandler ;
        
        private ResRoleInfoMessage result;
        public void handler(object msg)
        {

            result = NetManager.DeSerialize<ResRoleInfoMessage>((byte[])msg);
            PlayerManager.getInstance().RoleInfo = result;
            //MyEventArgs myArgs = new MyEventArgs(result);
            //RoleInfoHandler(this, myArgs);
            EventDispatcher.Instance().DispatchEvent("role_info_updata", result);
        }
    }
}
