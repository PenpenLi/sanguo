using UnityEngine;
using System.Collections;
using System.IO;

namespace org.alan {
    /// <summary>
    ///  应用程序管理器
    /// </summary>
    public static class ApplicationManager {
        public const string appConfigFile = "/config/app_config.json";
        public const string userMetaFile = "/meta/user.json";

        public static string path = Application.streamingAssetsPath;
        //应用程序配置文件
        public static AppConfig appConfig;
        //用户信息数据
        public static UserMeta userMeta;

        //在静态构造函数中初始化
        static ApplicationManager() {
            Debug.Log(path);
            string context = File.ReadAllText(path + appConfigFile);
            //读取应用配置
            appConfig = JsonUtility.FromJson<AppConfig>(context);

            string file = path + userMetaFile;
            if (!File.Exists(file)) {
                File.Create(file);
            }
            context = File.ReadAllText(file);
            //读取用户保存信息
            userMeta = JsonUtility.FromJson<UserMeta>(context);
            if (userMeta == null) {
                Debug.Log("没有加载到玩家账户信息");
                userMeta = new UserMeta();
            }

        }

        public static void SaveUserInfo(UserMeta _userMeta) {
            userMeta = _userMeta;
            string contents = JsonUtility.ToJson(userMeta);
            string file = path + userMetaFile;
            File.WriteAllText(file, contents);
        }


    }
}

