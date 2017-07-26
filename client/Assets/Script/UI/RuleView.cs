using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class RuleUIContext : BaseContext
    {
        public RuleUIContext() : base(UIType.RuleView) { }

        public List<RuleData> Datas;
    }

    public class RuleView : BaseView
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
            base.OnEnter(context);

            RuleUIContext theContext = context as RuleUIContext;

            ContentTxt.text = GetFullString(theContext.Datas);
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

        string GetFullString(List<RuleData> data)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                builder.Append("<size=45><b>").Append(data[i].Tile).Append("</b></size>").Append('\n');
                builder.Append("<size=35>").Append(data[i].Content).Append("</size>").Append("\n\n");
            }

            return builder.ToString();
        }
    }
}
