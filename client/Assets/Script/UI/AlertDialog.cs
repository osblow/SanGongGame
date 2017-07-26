using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Osblow.App
{
    public class AlertUIContext : BaseContext
    {
        public AlertUIContext() : base(UIType.AlertDialog) { }

        public string Info;
        public bool HasCancel = false;
        public bool HasOK = false;
        public System.Action CancelCallback;
        public System.Action OKCallback;
    }

    public class AlertDialog : BaseView
    {
        #region 场景引用 
        public Text Info;

        public GameObject OKButton;
        public GameObject CancelButton;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Pop();
        }

        public void OnOKBtn()
        {
            if(OKCallback != null)
            {
                OKCallback.Invoke();
            }
            Pop();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnCancelBtn()
        {
            if(CancelCallback != null)
            {
                CancelCallback.Invoke();
            }

            Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        private System.Action OKCallback;
        private System.Action CancelCallback;



        void Pop()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
        }



        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            AlertUIContext theContext = context as AlertUIContext;
            Info.text = theContext.Info;

            OKButton.SetActive(theContext.HasOK);
            CancelButton.SetActive(theContext.HasCancel);

            OKCallback = theContext.OKCallback;
            CancelCallback = theContext.CancelCallback;
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