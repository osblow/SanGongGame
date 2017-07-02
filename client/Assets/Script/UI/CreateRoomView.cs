using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class CreateRoomUIContext : BaseContext
    {
        public CreateRoomUIContext() : base(UIType.CreateRoomView) { }
    }

    public class CreateRoomView : BaseView
    {
        #region 场景引用 

        #endregion

        #region 场景事件
        public void OnCreateBtn()
        {
            OnCreateRoom();
        }
        #endregion
        void OnCreateRoom()
        {
            Globals.SceneSingleton<ContextManager>().Push(new TableUIContext());
        }


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