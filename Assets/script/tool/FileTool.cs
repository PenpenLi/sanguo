using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

namespace Assets.Scripts.tool {
    class FileTool
    {
        /// <summary> 可读写配置 文件位置 </summary>
        static public string WRITE_READ_CONFIG_PATH = Application.persistentDataPath;
        /// <summary> 可读写配置 文件名 </summary>
        static public string WRITE_READ_CONFIG_NAME = "config.txt";
        /// <summary> 可读配置 文件名 </summary>
        static public string READ_CONFIG_NAME = "gameconfig.yaml";
        /// <summary> 总金币数 </summary>
        static public string TOTAL_POINT = "grade";
        /// <summary> 最高分 </summary>
        static public string HIGHTEST_POINT = "mostTotalPoint";
        /// <summary> username </summary>
        static public string USERNAME = "user_name";
        /// <summary> password </summary>
        static public string PASSWORD = "password";


        static public string LoadXML(string path)
        {
            string result = Resources.Load(path).ToString();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            return result;
        }

        /// <summary>
        /// 写数据 config 全部数据
        /// </summary>
        /// <param name="path">文件创建目录</param>
        /// <param name="name"></param>文件的名称
        /// <param name="info"></param>写入的内容
        static public void WriteFile(string path, string name, ArrayList info)
        {
            //文件流信息
            StreamWriter sw = null;
            FileInfo t = new FileInfo(path + "//" + name);
            DeleteFile(path, name);
            // sw = t.CreateText();
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
            }
            else
            {
                //如果此文件存在则打开
                sw = t.AppendText();

            }
            //以行的形式写入信息
            for (int i = 0; i < info.Count; i++)
            {
                sw.WriteLine(info[i]);
            }


            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
        }
        /// <summary>
        /// 写数据 config   不必是全部数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="info"></param>
        static public void WriteFileSingle(string path, string name, ArrayList info)
        {
            ArrayList dataArr = ReadFile(path,name);
            ArrayList addArr = new ArrayList();
           for (int i = 0; i < info.Count; i++)
            {
                string key = info[i].ToString().Split(':')[0];
                bool isExist = false;
                for(int j = 0;j<dataArr.Count;j++)
                {
                    if (key == dataArr[j].ToString().Split(':')[0])
                    {
                        isExist = true;
                        dataArr[j] = key+ ":" + info[i].ToString().Split(':')[1];
                        break;
                    }
                }
                if(!isExist)
                {
                    addArr.Add(info[i]);
                }
            }
           for(int i = 0;i<addArr.Count;i++)
            {
                dataArr.Add(addArr[i]);
            }
            WriteFile(path,name,dataArr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        static public string readFileProp(string path, string name,string propName)
        {
            ArrayList arrlist = ReadFile(path, name);
            for (int i = 0; i < arrlist.Count; i++)
            {
                string[] arr = arrlist[i].ToString().Split(':');
                if (arr[0] == propName)
                {
                    return arr[1];
                }
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>读取文件的路径
        /// <param name="name"></param>读取文件的名称
        /// <returns></returns>
        static private ArrayList ReadFile(string path, string name )
        {
            ArrayList arrlist = new ArrayList();
            //使用流的形式读取
            StreamReader sr = null;
            FileInfo t = new FileInfo(path + "//" + name);
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                t.CreateText();
            }
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                //路径与名称未找到文件则直接返回空
                return arrlist;
            }
            string line;
            
            while ((line = sr.ReadLine()) != null)
            {
                //一行一行的读取
                //将每一行的内容存入数组链表容器中
                arrlist.Add(line);
            }
            //关闭流
            sr.Close();
            //销毁流
            sr.Dispose();
            //将数组链表容器返回
            return arrlist;
        }

        /// <summary>
        /// 删除文件 <para> path:路径</para>
        /// </summary>
        /// <param name="path"></param>删除文件的路径
        /// <param name="name"></param>删除文件的名称
        static public void DeleteFile(string path, string name)
        {
            File.Delete(path + "//" + name);

        }

        static public void readSystemConfig(string str)
        {
            if(str !="")
            {
               // Debug.Log("s");
                //"﻿aaa:123\r\nbbb:12344"
                str = str.Replace("\r\n", "\n");
                string[] arr = str.Split(new char[] { '\n' });
                for(int i = 1;i<arr.Length;i++)
                {
                    string[] parms = arr[i].Split(new char[] { ':' });
                    switch (parms[0])
                    {
                        //case "game.reviveNeedDiamond":
                        //    GameConfig.reviveNeedDiamond = int.Parse(parms[1]);
                        //    break;
                        //case "game.renameNeedDiamond":
                        //    GameConfig.renameNeedDiamond = int.Parse(parms[1]);
                        //    break;
                        //case "game.roleNameLength":
                        //    GameConfig.roleNameLength = int.Parse(parms[1]);
                        //    break;
                    }
                }
                
                //Debug.Log(GameConfig.roleNameLength);
            }
        }

        static public void readConfig()
        {
            //return;
            //本地路径  
            //Debug.Log(Application.streamingAssetsPath);
            //string fileAddress = System.IO.Path.Combine(Application.streamingAssetsPath, "prop1.csv");
            //FileInfo fInfo0 = new FileInfo(fileAddress);
            //string allStr = "";
            //if (fInfo0.Exists)
            //{
            //    StreamReader r = new StreamReader(fileAddress);
            //    //StreamReader默认的是UTF8的不需要转格式了，因为有些中文字符的需要有些是要转的，下面是转成String代码  
            //    byte[] data = new byte[1024];  
            //     data = Encoding.UTF8.GetBytes(r.ReadToEnd());  
            //     allStr = Encoding.UTF8.GetString(data, 0, data.Length);
            //    // s = r.ReadToEnd();
            //    //  te.text = s;
            //    allStr = allStr.Replace("\r\n", "\n");
            //    string[] strline = allStr.Split(new char[] { '\n' });
            //    Debug.Log(allStr);
            //}


            //string lua = @"DB_PROP ={
            //    { sid = 1, propId = 101, propName = '合金矿镐'}
            //}
            //";
            ////创建 lua 状态对象
            //LuaState luaState = new LuaState();
            ////运行脚本确保函数已经创建
            //luaState.DoString(lua);
            ////获取函数
            //// LuaFunction func = luaState.GetTable("DB_PROP");
            ////调用函数
            ////  object[] result = func.Call(5.2f, 1.3f);
            ////打印结果
            ////  Debug.Log(result[0]);


            //LuaTable table = luaState.GetTable("DB_PROP");
            //for (int i = 0; i < table.Count; i++)
            //{
            //    Debug.Log(table[i]);
            //}


            

        }
    }
}
