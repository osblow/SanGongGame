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
        public Text Number;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnConfirmBtn()
        {
            if (string.IsNullOrEmpty(m_roomId))
            {
                return;
            }

            Globals.SceneSingleton<ContextManager>().WebBlockUI(true);
            HttpRequest.ExistRoomRequest(m_roomId);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnClickNumber(int num)
        {
            SetNumber(num);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnClickReset()
        {
            ResetNum();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnDeleteOne()
        {
            DeleteOne();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion


        int m_curNumIndex = 0;
        const int c_maxNumCount = 6;
        string m_roomId = "";
        void SetNumber(int num)
        {
            if(m_curNumIndex > c_maxNumCount)
            {
                return;
            }

            ++m_curNumIndex;
            m_roomId += num.ToString();
            Number.text += "  " + num.ToString();

            if (m_curNumIndex >= c_maxNumCount)
            {
                Globals.SceneSingleton<ContextManager>().WebBlockUI(true);
                HttpRequest.ExistRoomRequest(m_roomId);
                return;
            }
        }

        void ResetNum()
        {
            Number.text = "";
            m_roomId = "";
            m_curNumIndex = 0;
        }

        void DeleteOne()
        {
            if(Number.text.Length <= 0)
            {
                return;
            }

            Number.text = Number.text.Remove(Number.text.Length - 3);
            m_roomId = m_roomId.Remove(m_roomId.Length - 1);
            --m_curNumIndex;
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
