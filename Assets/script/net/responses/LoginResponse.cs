using Assets.Scripts.login;
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
    class LoginResponse : IResponse
    {
        protected ResLoginResultMessage result;
        public void handler(object msg)
        {
            result = NetManager.DeSerialize<ResLoginResultMessage> ((byte[])msg); 
             Debug.Log(result.login_result);
            PlayerManager.getInstance().LoginResult = result;
            
        }

        
    }
}
