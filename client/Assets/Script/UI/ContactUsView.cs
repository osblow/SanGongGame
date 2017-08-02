using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Util;

namespace Osblow.App
{
    public class ContactUsUIContext : BaseContext
    {
        public ContactUsUIContext() : base(UIType.ContactUsView) { }

        public List<ContactUsData> Datas;
    }

    public class ContactUsView : BaseView
    {
        #region 场景引用 
        public List<Text> Names;
        public List<Text> Values;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        private void OnRecieveMessage(Msg msg)
        {
            List<ContactUsData> datas = msg.Get<List<ContactUsData>>(0);
            for (int i = 0; i < datas.Count && i < Names.Count; i++)
            {
                Names[i].text = datas[i].Key;
                Values[i].text = datas[i].Value;
            }
        }


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            MsgMng.AddListener(MsgType.OnRecieveContactUsMessage, OnRecieveMessage);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            MsgMng.RemoveListener(MsgType.OnRecieveContactUsMessage, OnRecieveMessage);
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