using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Osblow.App
{
    public class ContactUsUIContext : BaseContext
    {
        public ContactUsUIContext() : base(UIType.ContactUsView) { }

        public List<ContactUsData> Datas;
    }

    public class ContactUsView : BaseView
    {
        #region 场景引用 
        public List<Text> Names;
        public List<Text> Values;
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
            base.OnEnter(context);

            ContactUsUIContext theContext = context as ContactUsUIContext;
            for (int i = 0; i < theContext.Datas.Count && i < Names.Count; i++)
            {
                Names[i].text = theContext.Datas[i].Key;
                Values[i].text = theContext.Datas[i].Value;
            }
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