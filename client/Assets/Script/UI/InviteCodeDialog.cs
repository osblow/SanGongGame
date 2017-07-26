using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
            InviteCodeUIContext theContext = context as InviteCodeUIContext;
            if (theContext.Binded)
            {
                CodeInput.text = theContext.BindedCode.ToString();
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

            NoticeTxt.text = theContext.Notice;
            base.OnEnter(context);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
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
