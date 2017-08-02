using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Osblow.App
{
    public class Globals : MonoBehaviour
    {
        private static Globals s_instance;
        public static Globals Instance { get { return s_instance; } }
        private void Awake()
        {
            s_instance = this;

            //Application.logMessageReceived += LogCallback;

            Application.targetFrameRate = 45;

            littlerbird.units.LogManager.openDebug(gameObject);
        }
        
        List<string> m_logLines = new List<string>();
        void LogCallback(string logString, string stackTrace, LogType type)
        {
            if(type == LogType.Warning)
            {
                return;
            }
            
            string newLog = System.DateTime.Now + "   " + logString;
            m_logLines.Add(newLog);
        }

        public void LogCallbackResponse(string log)
        {
            string newLog = "response    " + System.DateTime.Now + "   " + log;
            m_logLines.Add(newLog);
        }

        public void LogCallbackRequest(string log)
        {
            string newLog = "request:   " + System.DateTime.Now + "   " + log;
            m_logLines.Add(newLog);
        }

        private const string c_fileName = "SangongLog.txt";
        public void SaveLog()
        {
            string path = Application.persistentDataPath + "//" + c_fileName;

            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path);
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
            for (int i = 0; i < m_logLines.Count; i++)
            {
                sw.WriteLine(m_logLines[i]);
            }

            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
            m_logLines.Clear();

            AlertUIContext context = new AlertUIContext();
            context.Info = "已保存日志文件到： " + path;
            Globals.SceneSingleton<ContextManager>().Push(context);
        }









        public int ReconnectTime = 10;


        private static Dictionary<Type, MonoBehaviour> s_singletonDic 
            = new Dictionary<Type, MonoBehaviour>();

        public static T SceneSingleton<T>()
            where T : MonoBehaviour
        {
            if (!s_singletonDic.ContainsKey(typeof(T)))
            {
                GameObject temp = new GameObject(typeof(T).ToString());
                T t = temp.AddComponent<T>();
                temp.transform.SetParent(s_instance.transform);
                s_singletonDic.Add(typeof(T), t);
            }

            return (T)s_singletonDic[typeof(T)];
        }

        public static void RemoveSceneSingleton<T>()
        {
            
            if (!s_singletonDic.ContainsKey(typeof(T)))
            {
                return;
            }

            GameObject.Destroy(s_singletonDic[typeof(T)].gameObject);
            s_singletonDic[typeof(T)].StopAllCoroutines();
            s_singletonDic.Remove(typeof(T));
            
        }


        public Settings Settings = new Settings();


        public static void GetHeadImgByUrl(string url, Action<Texture> getTextureCallback)
        {
            Instance.StartCoroutine(GetImageByUrl(url, getTextureCallback));
        }

        private static IEnumerator GetImageByUrl(string url, Action<Texture> getTextureCallback)
        {
            WWW www = new WWW(url);
            yield return www;
            if (getTextureCallback != null)
                getTextureCallback.Invoke(www.texture);
        }
    }

    public class Settings
    {
        //sangong.sansanbbox.com
        //public string WebUrlBase = "http://bbox.sansanbbox.com:6080/";
        //public string SocketUrl = "112.74.89.125";

        public string WebUrlBase = "http://sangong.sansanbbox.com/sangong/";
        public string SocketUrl = "120.77.245.34";

        public int SocketPort = 9876;



        public int ChatSocketPort = 9877;
    }
}

