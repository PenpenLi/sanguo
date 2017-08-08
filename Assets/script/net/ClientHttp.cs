using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.net {
    class ClientHttp {
        private float mJindu;

        public string mContent { get; private set; }
        private bool isBegin;

        private static ClientHttp instance;
        public static ClientHttp getInstance()//获取实例化对象
        {
            if (instance == null) {
                instance = new ClientHttp();
            }
            return instance;
        }


        //POST请求(Form表单传值、效率低、安全 ，)  
        public IEnumerator POST(string url, Dictionary<string, string> post, Action<string> callBack) {
            Debug.Log("POST");
            // if (isBegin) yield return null;
            mContent = "";
            isBegin = true;
            //表单   
            WWWForm form = new WWWForm();
            //从集合中取出所有参数，设置表单参数（AddField()).  
            foreach (KeyValuePair<string, string> post_arg in post) {
                form.AddField(post_arg.Key, post_arg.Value);
            }

            //表单传值，就是post   
            WWW www = new WWW(url, form);
            while (!www.isDone) {
                Debug.Log("网络请求进程,url=" + url + ",progress=" + www.progress);
                yield return www;
            }

            if (www.error != null && !www.error.Equals("")) {
                //POST请求失败  
                mContent = "error :" + www.error;
            } else {
                //POST请求成功  
                mContent = www.text;
            }
            //Debug.Log(mContent);
            callBack(mContent);
            isBegin = false;
        }



        //GET请求（url?传值、效率高、不安全 ）  
        public IEnumerator GET(string url, Dictionary<string, string> get) {
            string Parameters;
            bool first;
            if (get.Count > 0) {
                first = true;
                Parameters = "?";
                //从集合中取出所有参数，设置表单参数（AddField()).  
                foreach (KeyValuePair<string, string> post_arg in get) {
                    if (first)
                        first = false;
                    else
                        Parameters += "&";

                    Parameters += post_arg.Key + "=" + post_arg.Value;
                }
            } else {
                Parameters = "";
            }

            string testC = "getURL :" + Parameters;

            //直接URL传值就是get  
            WWW www = new WWW(url + Parameters);
            while (!www.isDone) {
                Debug.Log("网络请求进程,url=" + url + ",progress=" + www.progress);
                yield return www;
            }
            if (www.error != null && !www.error.Equals("")) {
                //POST请求失败  
                mContent = "error :" + www.error;
            } else {
                //POST请求成功  
                mContent = www.text;
            }
        }
    }
}
