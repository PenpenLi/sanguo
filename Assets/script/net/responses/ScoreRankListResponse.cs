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
    class ScoreRankListResponse : IResponse
    {
        static public event events.MyEventArgs.MyHandler SroceRankHandler ;

        private ResScoreRanklist result;
        public void handler(object msg)
        {

            result = NetManager.DeSerialize<ResScoreRanklist>((byte[])msg);

            MyEventArgs myArgs = new MyEventArgs(result);
            SroceRankHandler(this, myArgs);
          //  Debug.Log(result);

        }
    }
}
