using Assets.Scripts.events;
using Assets.Scripts.manager;
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
    class PingResponse : IResponse
    {
       // static public event events.MyEventArgs.MyHandler RoleInfoHandler ;

        private ResPingMessage result;
        public void handler(object msg)
        {

            result = NetManager.DeSerialize<ResPingMessage>((byte[])msg);
            //Debug.Log(result.ping);
            //MyEventArgs myArgs = new MyEventArgs(result);
            //RoleInfoHandler(this, myArgs);
        }
    }
}
