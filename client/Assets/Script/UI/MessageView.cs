using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Util;


namespace Osblow.App
{
    public class MessageUIContext : BaseContext
    {
        public MessageUIContext() : base(UIType.MessageView) { }
    }

    public class MessageView : BaseView
    {
        #region 场景引用 
        public Text Title;
        public Text ContentTxt;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        private void OnReceiveMessage(Msg msg)
        {
            MessageData lobbyData = msg.Get<MessageData>(0);

            Title.text = lobbyData.NewsTitle;
            ContentTxt.text = lobbyData.News;
        }


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            MsgMng.AddListener(MsgType.OnRecieveMessage, OnReceiveMessage);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            MsgMng.RemoveListener(MsgType.OnRecieveMessage, OnReceiveMessage);
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }
    }
}
