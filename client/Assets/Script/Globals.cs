using System;
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
        }


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

            //GameObject.Destroy(s_singletonDic[typeof(T)].gameObject);
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
        public string WebUrlBase = "http://183.61.146.92:81/sangong/";//"http://bbox.sansanbbox.com:6080/";
        public string SocketUrl = "183.61.146.92";
        public int SocketPort = 9876;
    }
}

