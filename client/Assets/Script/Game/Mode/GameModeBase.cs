using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Osblow.Util;
using Osblow.Game;

namespace Osblow.App
{
    public class GameModeBase : MonoBehaviour
    {
        enum Step
        {
            Init = 0,
            GettingReady = 1,
            Started = 2,
            Betting =3,
            Bankering = 4,
            DealingCards = 5,
            ShowCards = 6,
            Result = 7
        }

        Step m_curStep = Step.Init;

        public void Init()
        {
            Globals.SceneSingleton<GameMng>();

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
                //Globals.SceneSingleton<ContextManager>().Push(new TableUIContext());
            };

            CmdRequest.EnterRoomRequest();
        }

        void OnGetReady(Msg msg)
        {
            string uuid = msg.Get<string>(0);
        }
    }
}
