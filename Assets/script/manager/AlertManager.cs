using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.manager
{
    class AlertManager
    {
        static private AlertManager _instance;
        static public AlertManager getInstance()
        {
            if (_instance == null)
            {
                _instance = new AlertManager();
            }
            return _instance;
        }

        static public AlertManager instance;

        public enum AlertType
        {
            OK,
            OKCancel,
            Custom1Btn,         // 自定义一个按钮  
            Custom2Btn,         // 自定义两个按钮  
        };
        static private string getResourceName(AlertType type)
        {
            string resourceName = "";
            switch (type)
            {
                case AlertType.OK:
                    resourceName = PopupWindowName.NOTICE_WINDOW_A;
                    break;
                case AlertType.OKCancel:
                    resourceName = PopupWindowName.NOTICE_WINDOW_B;
                    break;
            }
            return resourceName;
        }

       // public delegate void OkBtnCallback();
       // public delegate void CancelBtnCallback();
       /// <summary>
       /// 
       /// </summary>
       /// <param name="type"></param>
       /// <param name="text"></param>
       /// <param name="titleText"></param>
       /// <param name="okCallback"></param>
       /// <param name="cancelCallback"></param>
        static public void add(AlertType type, string text = "", string titleText = "", Action okCallback = null, Action cancelCallback = null)
        {
            GameObject go = PopupManager.AddWindow(getResourceName(type));
            if (titleText != "")
                go.transform.Find("Macth/TitleText").gameObject.GetComponent<Text>().text = titleText;
            go.transform.Find("Macth/Text").gameObject.GetComponent<Text>().text = text;
            go.transform.Find("Macth/BtnOk").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
                okCallback();
            });
            if(go.transform.Find("Macth/BtnCancel") != null)
            {
                go.transform.Find("Macth/BtnCancel").gameObject.GetComponent<Button>().onClick.AddListener(delegate {
                    cancelCallback();
                });
            }

        }
        static public void remove(AlertType type)
        {
            PopupManager.RemoveWindow(getResourceName(type));
        }
    }


}
