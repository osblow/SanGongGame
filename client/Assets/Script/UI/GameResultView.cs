using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class GameResultUIContext : BaseContext
    {
        public GameResultUIContext() : base(UIType.GameResultView) { }

        public List<ResultUser> Results;
    }

    public class GameResultView : BaseView
    {
        #region 场景引用 
        public GameObject GridRoot;
        public GameObject ItemTpl;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<UIManager>().DestroySingleUI(UIType.TableView);
            Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());

            Globals.SceneSingleton<SoundMng>().StopBackSound();
            Globals.RemoveSceneSingleton<Osblow.Game.SocketNetworkMng>();

            Globals.SceneSingleton<StateMng>().ChangeState(StateType.Lobby);

            Globals.Instance.SaveLog();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion

        private List<GameObject> m_items = new List<GameObject>();

        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            GameResultUIContext theContext = context as GameResultUIContext;
            for (int i = 0; i < theContext.Results.Count; i++)
            {
                GameObject newItem = Instantiate(ItemTpl);
                newItem.transform.GetChild(1).GetComponent<Text>().text = theContext.Results[i].Name;
                newItem.transform.GetChild(2).GetComponent<Text>().text = theContext.Results[i].Point.ToString();

                newItem.transform.SetParent(GridRoot.transform, false);
                newItem.SetActive(true);

                m_items.Add(newItem);
            }
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            for (int i = 0; i < m_items.Count; i++)
            {
                Destroy(m_items[i]);
            }

            m_items.Clear();
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
