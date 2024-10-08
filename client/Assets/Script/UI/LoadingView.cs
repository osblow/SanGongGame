﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class LoadingUIContext : BaseContext
    {
        public LoadingUIContext() : base(UIType.LoadingView) { }

        public string Info;
    }

    public class LoadingView : BaseView
    {
        #region 场景引用 
        public Text Info;
        #endregion

        #region 场景事件
        
        #endregion
        


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            Info.text = (context as LoadingUIContext).Info;
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

