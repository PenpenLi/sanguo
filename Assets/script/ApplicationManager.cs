using UnityEngine;
using System.Collections;
using System.IO;
using Assets.script.constant;

namespace org.alan {

    /// <summary>
    ///  应用程序管理器
    /// </summary>
    public class ApplicationManager : MonoBehaviour {
        public const string appConfigFile = "/app_config.json";
        public const string userMetaFile = "/user.json";
        //应用程序配置文件
        public static AppConfig appConfig;
        //用户信息数据
        public static UserMeta userMeta;

        public bool done = false;

        public static void SaveUserInfo(UserMeta _userMeta) {
            userMeta = _userMeta;
            string contents = JsonUtility.ToJson(userMeta);
            System.IO.Directory.CreateDirectory(Application.persistentDataPath);
            string file = Application.persistentDataPath + userMetaFile;
            Debug.LogFormat("保存用户信息，file={0}", file);
            FileInfo t = new FileInfo(file);
            t.Delete();
            //文件流信息
            StreamWriter sw = null;
            // sw = t.CreateText();
            if (!t.Exists) {
                //如果此文件不存在则创建
                sw = t.CreateText();
            } else {
                //如果此文件存在则打开
                sw = t.AppendText();

            }
            sw.Write(contents);
            sw.Close();
            sw.Dispose();
        }
        //在静态构造函数中初始化
        private void Start() {
            LoadAll();
            StartCoroutine(SceneTool.LoadScene("scene/login"));
        }

        private void LoadAll() {
            Debug.Log("开始加载配置文件...");
            Debug.Log(ConfigConst.StreamingAssets);
            StartCoroutine(LoadConfigFile(ConfigConst.StreamingAssets + appConfigFile));
            //从持久化地址加载用户保存信息
            string file = Application.persistentDataPath + userMetaFile;
            StartCoroutine(LoadUserFile(file));
        }

        public IEnumerator LoadConfigFile(string fileUrl) {
            WWW www = new WWW(fileUrl);
            Debug.Log(www.url);
            if (!www.isDone) {
                yield return www;
            }
            string context = www.text;
            appConfig = JsonUtility.FromJson<AppConfig>(context);
        }


        public IEnumerator LoadUserFile(string fileUrl) {
            WWW www = new WWW(fileUrl);
            if (!www.isDone) {
                yield return www;
            }
            string context = www.text;
            if (string.IsNullOrEmpty(context)) {
                Debug.Log("没有加载到玩家账户信息");
                userMeta = new UserMeta();
            } else {
                userMeta = JsonUtility.FromJson<UserMeta>(context);
            }
        }

        /// <summary>
        /// 删除文件 <para> path:路径</para>
        /// </summary>
        /// <param name="path"></param>删除文件的路径
        /// <param name="name"></param>删除文件的名称
        static public void DeleteFile(string path, string name) {
            File.Delete(path + "//" + name);

        }


        private void OnApplicationQuit() {
            Debug.Log("login OnApplicationQuit");
            if (NetManager.clientSocket != null) {
                NetManager.clientSocket.Close();
            }
        }
    }
}

