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

