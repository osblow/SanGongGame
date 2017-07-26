using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class MessageUIContext : BaseContext
    {
        public MessageUIContext() : base(UIType.MessageView) { }

        public MessageData Data;
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



        public override void OnEnter(BaseContext context)
        {
            MessageUIContext theContext = context as MessageUIContext;
            Title.text = theContext.Data.NewsTitle;
            ContentTxt.text = theContext.Data.News;

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
