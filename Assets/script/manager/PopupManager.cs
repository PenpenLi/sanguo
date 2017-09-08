using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.manager {
    class PopupManager : MonoBehaviour {

        private static Dictionary<string, GameObject> dicPopup = new Dictionary<string, GameObject>();
        public static void ShowClosePopUp(string message) {
            GameObject gameObject = AddWindow(PopupWindowName.CLOSE_POP_UP);
            ClosePopPanelController controller = gameObject.GetComponent<ClosePopPanelController>();
            controller.message.text = message;
        }

        public static void ShowTimerPopUp(string message) {
            GameObject gameObject = AddWindow(PopupWindowName.TIME_POP_UP);
            TimePopPanelController controller = gameObject.GetComponent<TimePopPanelController>();
            controller.message.text = message;
        }

        /// <summary>
        /// 弹出窗口
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="tranParent"></param>
        public static GameObject AddWindow(string resourceName, Transform tranParent = null) {
            Debug.Log("showWindow：" + resourceName);
            //if (dicPopup.Keys.Contains(resourceName))//repeat
            //{
            //    Debug.Log("resource is already popup");
            //    return null;
            //}
            GameObject goPre = (GameObject)Resources.Load(resourceName);
            if (goPre == null) {
                Debug.Log("resourceName error");
                return null;
            }
            if (tranParent == null) {
                tranParent = GameObject.Find("CanvasPopUp").transform;
            }
            GameObject go = (GameObject)Instantiate(goPre, tranParent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1, 1, 1);
            // go.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            //go.transform.rectTransform().offsetMin = new Vector2(0, 0);
            //go.transform.rectTransform().offsetMax = new Vector2(0, 0);
            if (dicPopup.Keys.Contains(resourceName)) {
                RemoveWindow(resourceName);
            }
            dicPopup.Add(resourceName, go);
            return go;
        }

        /// <summary>
        /// 移除 窗口
        /// </summary>
        /// <param name="resourceName"></param>
        public static void RemoveWindow(string resourceName) {
            Debug.Log("removeWindow：" + resourceName);
            if (dicPopup.Keys.Contains(resourceName)) {
                Destroy(dicPopup[resourceName]);
                dicPopup.Remove(resourceName);
            } else {
                Debug.Log("resourceName error");
                return;
            }
        }
    }



    class PopupWindowName {
        /// <summary>
        /// 匹配框
        /// </summary>
        static public string MATCH_POP_UP = "SceneUI/matchPanel";
        /// <summary>
        /// 定时关闭弹出框
        /// </summary>
        static public string TIME_POP_UP = "SceneUI/TimerPopUp";
        /// <summary>
        /// 需要点击关闭的弹出框
        /// </summary>
        static public string CLOSE_POP_UP = "SceneUI/ClosePopUp";
        /// <summary>
        /// 提示 确定
        /// </summary>
        static public string NOTICE_WINDOW_A = "SceneUI/NoticePlaneA";
        /// <summary>
        /// 提示 确定,取消
        /// </summary>
        static public string NOTICE_WINDOW_B = "SceneUI/NoticePlaneB";
        /// <summary>
        /// waiting
        /// </summary>
        static public string WAITING_NET = "SceneUI/Waiting";
        /// <summary>
        /// task
        /// </summary>
        static public string TASK_PANEL = "SceneUI/QuestPlane";
        /// <summary>
        /// storePlane
        /// </summary>
        static public string STORE_PANEL = "SceneUI/EquipPlane";
        /// <summary>
        /// buyPlane
        /// </summary>
        static public string BUY_PANEL = "SceneUI/BuyPlane";
        /// <summary>
        /// relive
        /// </summary>
        static public string RELIVE_PANEL = "SceneUI/Relive";
        /// <summary>
        /// loading
        /// </summary>
        static public string LOADING_PANEL = "SceneUI/LoadingPlane";
        /// <summary>
        /// season
        /// </summary>
        static public string SEASON_PANEL = "SceneUI/SeasonPlanel";
    }

}
