using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class TableUIContext : BaseContext
    {
        public TableUIContext() : base(UIType.TableView) { }
    }

    public class TableView : BaseView
    {
        #region 场景引用 
        public List<GameObject> UserPanels;
        #endregion

        #region 场景事件
        public void OnMenuBtn()
        {
            ShowMenu();
        }

        public void OnCuoPai()
        {
            CuoPai();
        }
        #endregion
        void ShowMenu()
        {
            Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
        }

        void CuoPai()
        {

        }

        void ShowCards()
        {

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