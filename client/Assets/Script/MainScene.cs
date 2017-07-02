using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Osblow.App
{
    public class MainScene : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            Globals.SceneSingleton<UIManager>();
            Globals.SceneSingleton<ContextManager>();

            StartCoroutine(StartGame());
        }

        IEnumerator StartGame()
        {
            yield return new WaitForEndOfFrame();
            Globals.SceneSingleton<ContextManager>().Push(new LoginUIContext());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
