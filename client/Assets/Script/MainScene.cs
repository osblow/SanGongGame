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
            Globals.SceneSingleton<StateMng>();
            Globals.SceneSingleton<SoundMng>();

            StartCoroutine(StartGame());
        }

        IEnumerator StartGame()
        {
            yield return new WaitForEndOfFrame();
            Globals.SceneSingleton<StateMng>().ChangeState(StateType.Login);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
