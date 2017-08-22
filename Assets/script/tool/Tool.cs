using Assets.Scripts.manager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.tool {
    class Tool : MonoBehaviour {
        /// <summary>
        /// 带权重随机值
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int quzhongRandom(int[,] arr) {
            int faultValue = 1;
            float total = 0;
            int i;
            float sum = 0f;
            for (i = 0; i < arr.GetLength(0); i++) {
                total += arr[i, 1];
            }
            float ran = UnityEngine.Random.Range(0f, 1f);
            if (ran == 0)
                return faultValue;
            for (i = 0; i < arr.Length; i++) {
                sum += arr[i, 1] / total;
                if (ran <= sum) {
                    return arr[i, 0];
                }
            }
            return faultValue;
        }

        /// <summary>
        /// 红色提示
        /// </summary>
        /// <param name="n"></param>
        /// <param name="a"></param>
        /// <param name="isBig"></param>
        public static void redNotice(string n = "null", float a = 2, bool isBig = false) {
            string str;
            if (isBig) {
                str = "SceneUI/NoticeRedBig";
            } else {
                str = "SceneUI/NoticeRed";
            }
            GameObject noticePre = (GameObject)Resources.Load(str);
            Transform tran = GameObject.Find("EasyTouchControlsCanvas").transform.Find("NoticePos").transform;
            noticePre.GetComponent<Text>().text = n;
            GameObject notice = (GameObject)Instantiate(noticePre, tran);
            notice.transform.position = new Vector3(Screen.width / 2, Screen.height / a, 0);
        }

        /// <summary>
        /// 服务器返回Result，提示 解析
        /// </summary>
        /// <param name="result"></param>
        public static void serverNotice(GameResultEnum result) {
            int type = 2;
            switch (result) {
                case GameResultEnum.SUCCESS:
                    type = 1;
                    break;
            }
            noticeStandard(result.ToString(), null, type);
        }
        /// <summary>
        /// 一般 提示消息
        /// type =0:NoticeMoney  花钱成功的提示 1:NoticeGreenBig 普通成功的提示 2:NoticeRedBig 普通失败的提示
        /// </summary>
        /// <param name="str">name</param>
        /// <param name="displayparent"></param>
        public static void noticeStandard(string str, Transform displayparent = null, int type = 2, float destoryTime = 3.0f) {
            string resourcesName = "SceneUI/NoticeGreenBig";
            switch (type) {
                case 0:
                    resourcesName = "SceneUI/NoticeMoney";
                    break;
                case 1:
                    resourcesName = "SceneUI/NoticeGreenBig";
                    break;
                case 2:
                    resourcesName = "SceneUI/NoticeRedBig";
                    break;

            }

            //string preName = "SceneUI/SysNoticeA"; 
            GameObject noticePre = (GameObject)Resources.Load(resourcesName);
            GameObject notice;
            if (displayparent == null) {
                notice = (GameObject)Instantiate(noticePre, GameObject.Find("CanvasPopUp").transform);
            } else {
                notice = (GameObject)Instantiate(noticePre, displayparent);
            }

            notice.GetComponent<Text>().text = str;
            notice.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

            Destroy(notice, destoryTime);
        }

        /// <summary>
        /// 图片提示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="a"></param>
        public static void picNotice(string name = "null", float a = 350) {
            GameObject noticePre = (GameObject)Resources.Load(name);
            Transform tran = GameObject.Find("EasyTouchControlsCanvas").transform.Find("NoticePos").transform;

            GameObject notice = (GameObject)Instantiate(noticePre, tran);
            notice.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 + a, 0);
            notice.transform.localScale = new Vector3(1, 1, 1);
        }


        /// <summary>
        /// 删除 子节点
        /// </summary>
        /// <param name="tran"></param>
        public static void removeAllChild(Transform tran) {
            for (int i = 0; i < tran.childCount; i++) {
                Destroy(tran.GetChild(i).gameObject);
            }
        }
        public static GameObject getObjectResource(string path, Transform parentTran = null) {
            GameObject pre = (GameObject)Resources.Load(path);
            GameObject go = null;
            if (pre != null) {
                if (parentTran == null) {
                    go = (GameObject)Instantiate(pre);
                } else {
                    go = (GameObject)Instantiate(pre, parentTran);
                }

            } else {
                Debug.Log("pre path in not right!!!!!!");
                return null;
            }
            go.transform.localEulerAngles = new Vector3(0, 0, 0);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            return go;
        }

        public static bool isLoading = false;
        //注意这里返回值一定是 IEnumerator  
        /// <summary>
        /// 统一 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static IEnumerator LoadScene(string sceneName) {
            Debug.LogFormat("开始加载场景,scene name is {0}", sceneName);
            if (!isLoading) {
                isLoading = true;
                GameObject go = PopupManager.AddWindow(PopupWindowName.LOADING_PANEL);
                Text progress = go.transform.Find("TitleText").gameObject.GetComponent<Text>();
                AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
                while (!async.isDone) {
                    //  Debug.Log(async.progress);
                    progress.text = (int)(async.progress * 100) + "%";
                    yield return new WaitForEndOfFrame();//<strong>加上这么一句就可以先显示加载画面然后再进行加载</strong>  
                }
                //读取完毕后返回， 系统会自动进入C场景  
                yield return async;
            } else {
                Debug.Log("loading is acting !!! ");
                yield return null;
            }

        }
        public static long ToGMTTime(DateTime time) {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            return (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
        }
    }

}
