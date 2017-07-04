using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class EnterRoomUIContext : BaseContext
    {
        public EnterRoomUIContext() : base(UIType.EnterRoomView) { }
    }

    public class EnterRoomView : BaseView
    {
        #region 场景引用 

        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
        }

        public void OnConfirmBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new TableUIContext());
        }
        #endregion



        public override void OnEnter(BaseContext context)
        {
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
