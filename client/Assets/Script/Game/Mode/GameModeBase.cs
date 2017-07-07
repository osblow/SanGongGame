using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Osblow.Util;
using Osblow.Game;

namespace Osblow.App
{
    public class GameModeBase : MonoBehaviour
    {
        public void Init()
        {
            MsgMng.AddListener(MsgType.Connected, OnConnected);

            MsgMng.AddListener(MsgType.Registed, OnRegisted);
        }

        void OnConnected(Msg msg)
        {
            CmdRequest.ClientRegisterRequest();
        }

        void OnRegisted(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate
            {
                Globals.SceneSingleton<ContextManager>().Push(new TableUIContext());
            };
        }
    }
}
