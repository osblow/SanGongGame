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


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

