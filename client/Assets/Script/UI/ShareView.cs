﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class ShareUIContext : BaseContext
    {
        public ShareUIContext() : base(UIType.ShareView) { }
    }

    public class ShareView : BaseView
    {
        #region 场景引用 

        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnShareToFriendBtn()
        {
            WeixinHandler.ShareToFriend("啊哟啊哟", "http://www.baidu.com", "测试测试测试");
        }

        public void OnShareToTimelineBtn()
        {
            WeixinHandler.ShareToTimeline("啊哟啊哟", "http://www.baidu.com", "测试测试测试");
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
