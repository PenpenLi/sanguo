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
    class CreatRoleResponse : IResponse
    {
        static public event events.MyEventArgs.MyHandler CreateRoleEnd;
        private ResCreateResultMessage result;
        public void handler(object msg)
        {
            result = NetManager.DeSerialize<ResCreateResultMessage>((byte[])msg);
            Debug.Log(result.create_result);
            MyEventArgs myArgs = new MyEventArgs(result);
            CreateRoleEnd(this, myArgs);
        }

    }
}
