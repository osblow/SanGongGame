using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Util;


namespace Osblow.App
{
    public class InviteCodeUIContext : BaseContext
    {
        public InviteCodeUIContext() : base(UIType.InviteCodeDialog) { }

        public bool Binded = false;
        public string Notice = "";
        public uint BindedCode = 0;
    }

    public class InviteCodeDialog : BaseView
    {
        #region 场景引用 
        public Text NoticeTxt;
        public InputField CodeInput;
        public Button ConfirmBtn;
        #endregion
        private void OnRecieveInviteCode(Msg msg)
        {
            bool binded = msg.Get<bool>(0);
            uint bindedCode = msg.Get<uint>(1);
            string notice = msg.Get<string>(2);

            if (binded)
            {
                CodeInput.text = bindedCode.ToString();
                CodeInput.GetComponent<Image>().color = Color.yellow;
                CodeInput.enabled = false;
                ConfirmBtn.interactable = false;
            }
            else
            {
                CodeInput.text = "";
                CodeInput.GetComponent<Image>().color = Color.white;
                CodeInput.enabled = true;
                ConfirmBtn.interactable = true;
            }

            NoticeTxt.text = notice;
        }


        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnConfirmBtn()
        {
            HttpRequest.BindInviteCodeRequest(CodeInput.text);
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            MsgMng.AddListener(MsgType.OnRecieveInviteCode, OnRecieveInviteCode);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            MsgMng.RemoveListener(MsgType.OnRecieveInviteCode, OnRecieveInviteCode);
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
