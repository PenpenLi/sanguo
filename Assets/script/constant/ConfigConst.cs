using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.script.constant {
   public class ConfigConst {
        //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。  
        public static readonly string StreamingAssets =
#if UNITY_ANDROID   //安卓  
        "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE  //iPhone  
        Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台  
    "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;  
#endif
    }
}
