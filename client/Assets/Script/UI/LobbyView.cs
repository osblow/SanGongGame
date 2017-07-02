using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class LobbyUIContext : BaseContext
    {
        public LobbyUIContext() : base(UIType.LobbyView) { }
    }

    public class LobbyView : BaseView
    {
        #region 场景引用 

        #endregion

        #region 场景事件
        public void CreateRoomBtn()
        {
            ShowCreateView();
        }
        #endregion
        void ShowCreateView()
        {
            Globals.SceneSingleton<ContextManager>().Push(new CreateRoomUIContext());
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