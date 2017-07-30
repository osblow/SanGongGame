using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class GameLoadingUIContext : BaseContext
    {
        public GameLoadingUIContext() : base(UIType.GameLoadingView) { }

        public System.Action ProgressEndCallback;
    }

    public class GameLoadingView : BaseView
    {
        #region 场景引用 
        public Image ProgressBar;
        #endregion

        #region 场景事件

        #endregion
        private Coroutine m_progressCoroutine = null;
        private bool m_canExit = false;
        private System.Action m_endCallback;


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            m_progressCoroutine = StartCoroutine(Progress());
            m_endCallback = ((GameLoadingUIContext)context).ProgressEndCallback;
        }

        public override void OnExit(BaseContext context)
        {
            m_canExit = true;
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }


        IEnumerator Progress()
        {
            float randomTimer = 0;
            float randomSpeed = 0.1f;

            float ratio = 0;
            while(ratio <= 1.0f)
            {
                ratio += randomSpeed * Time.deltaTime;
                ProgressBar.fillAmount = ratio;

                if (m_canExit)
                {
                    randomSpeed = 2.0f;
                }
                else
                {
                    randomTimer += Time.deltaTime;
                    if (randomTimer > 0.15f)
                    {
                        randomSpeed = Random.Range(0, 100) / 1000.0f;
                        randomTimer = 0;
                    }
                }
                yield return null;
            }

            while (true)
            {
                if (m_canExit)
                {
                    gameObject.SetActive(false);

                    if(null != m_endCallback)
                    {
                        m_endCallback.Invoke();
                    }

                    yield break;
                }

                yield return null;
            }
        }

        //private void Start()
        //{
        //    StartCoroutine(Progress());
        //}
    }
}
