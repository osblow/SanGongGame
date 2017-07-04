﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class LoadingUIContext : BaseContext
    {
        public LoadingUIContext() : base(UIType.LoadingView) { }
    }

    public class LoadingView : BaseView
    {
        #region 场景引用 

        #endregion

        #region 场景事件
        
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

