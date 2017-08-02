using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Game;
using Osblow.Util;


namespace Osblow.App
{
    public class HistoryDetailUIContext : BaseContext
    {
        public HistoryDetailUIContext() : base(UIType.HistoryDetail) { }

        public SmallGameRecord Data;
    }

    public class HistoryDetailView : BaseView
    {
        #region 场景引用 
        public Transform NamesRoot;
        public Transform TableRoot;
        public Transform TotalScoreRoot;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        private void OnRecieveRecords(Msg msg)
        {
            SmallGameRecord data = msg.Get<SmallGameRecord>(0);

            // 表头
            int i;
            for (i = 0; i < data.Players.Count; i++)
            {
                string name = data.Players[i].NickName;
                NamesRoot.GetChild(i).gameObject.SetActive(true);
                NamesRoot.GetChild(i).GetComponent<Text>().text = name;
            }
            for (int j = i; j < NamesRoot.childCount; j++)
            {

                NamesRoot.GetChild(j).gameObject.SetActive(false);
            }

            // 表
            if (data.SingleRecords.Count <= 0)
            {
                return;
            }

            int[] totalPoints = new int[data.SingleRecords[0].Players.Count];
            List<GameObject> items = new List<GameObject>();
            for (i = 0; i < data.SingleRecords.Count; i++)
            {
                GameObject newItem = Instantiate(Resources.Load(c_itemRootPath) as GameObject);
                newItem.transform.SetParent(TableRoot, false);
                items.Add(newItem);

                SingleRecord record = new SingleRecord(newItem, data.SingleRecords[i]);
                int nextIndex = i + 1;
                record.FoldOutBtn.onClick.AddListener(() => StartAnim(nextIndex));

                for (int j = 0; j < data.SingleRecords[i].Players.Count; j++)
                {
                    totalPoints[j] += data.SingleRecords[i].Players[j].Point;
                }
            }

            m_totalScores = TotalScoreRoot.GetComponentsInChildren<Text>();
            for (int j = 0; j < m_totalScores.Length; j++)
            {
                if (j < totalPoints.Length)
                {
                    m_totalScores[j].gameObject.SetActive(true);
                    m_totalScores[j].text = totalPoints[j].ToString();
                }
                else
                {
                    m_totalScores[j].gameObject.SetActive(false);
                }
            }

            // 动画
            if (null == m_anim)
            {
                m_anim = gameObject.AddComponent<HistoryDetailAnim>();
            }
            m_anim.Items = items;
            m_anim.GridCellHeight = TableRoot.GetComponent<GridLayoutGroup>().cellSize.y + 5;
            m_anim.MoveDist = m_anim.GridCellHeight * 2.5f;
            ////////////////////////////////////////////////
        }




        private HistoryDetailAnim m_anim = null;
        private const string c_itemRootPath = "Prefab/UI/HistoryView/roundItem1";
        private Text[] m_totalScores;

        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            MsgMng.AddListener(MsgType.OnRecieveHistoryDetail, OnRecieveRecords);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            MsgMng.RemoveListener(MsgType.OnRecieveHistoryDetail, OnRecieveRecords);
            Clear();
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }


        void Clear()
        {
            for (int i = TableRoot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(TableRoot.GetChild(i).gameObject);
            }
        }

        void StartAnim(int index)
        {
            m_anim.SetAnim(index);
        }
    }



    class SingleRecord
    {
        public GameObject RootObj;

        private List<Text> m_points;
        private Text m_index;
        private List<CardPanel> m_cards;
        public Button FoldOutBtn;
        private Text m_roundIndexTxt;

        private SingleSmallGameRecord m_record;

        public SingleRecord(GameObject obj, SingleSmallGameRecord record)
        {
            RootObj = obj;
            m_record = record;

            Init();
        }

        void Init()
        {
            m_points = new List<Text>();
            m_points.AddRange(RootObj.transform.Find("grid").GetComponentsInChildren<Text>());
            m_index = RootObj.transform.Find("index").GetComponent<Text>();
            FoldOutBtn = RootObj.transform.Find("Button").GetComponent<Button>();
            Transform cardPanel = RootObj.transform.Find("CardPanel");

            m_roundIndexTxt = RootObj.transform.Find("index").GetComponent<Text>();



            m_index.text = "第" + m_record.Index + "局";
            m_points.ForEach((x) => x.gameObject.SetActive(false));
            int i;
            
            for(i = 0; i < m_record.Players.Count; i++)
            {
                m_points[i].gameObject.SetActive(true);
                int thePoint = m_record.Players[i].Point;
                string numStr = thePoint > 0 ? "+" + thePoint.ToString() : thePoint.ToString();
                m_points[i].text = numStr;

                GameObject cp = RootObj.transform.Find("CardPanel").GetChild(i).gameObject;
                CardPanel p = new CardPanel(cp, m_record.Players[i]);
            }
            for (; i < RootObj.transform.Find("CardPanel").childCount; i++)
            {
                RootObj.transform.Find("CardPanel").GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    class CardPanel
    {
        public GameObject RootObj;
        private SingleCard[] m_cards;
        private GameObject m_betPanel;
        private Text m_betNumTxt;
        private GameObject m_resultBg;
        private Text m_result;
        private GameObject m_ownerTag;

        private SmallGameRecPlayer m_record;


        public CardPanel(GameObject obj, SmallGameRecPlayer record)
        {
            RootObj = obj;
            m_record = record;

            Init();
        }

        void Init()
        {
            m_cards = RootObj.transform.GetComponentsInChildren<SingleCard>();
            m_betPanel = RootObj.transform.Find("BetPanel").gameObject;
            m_betNumTxt = m_betPanel.transform.Find("bet").GetComponent<Text>();
            m_resultBg = RootObj.transform.Find("cardResult_bg").gameObject;
            m_result = RootObj.transform.Find("cardResult").GetComponent<Text>();
            m_ownerTag = RootObj.transform.Find("ownerTag").gameObject;

            m_ownerTag.SetActive(false);



            for (int i = 0; i < m_record.Cards.Count; i++)
            {
                m_cards[i].SetNum(m_record.Cards[i]);
            }
            m_result.text = CmdHandler.s_cardResultDic[m_record.CardResult];
            m_ownerTag.SetActive(m_record.IsBanker);
            m_betPanel.SetActive(!m_record.IsBanker);
            m_betNumTxt.text = m_record.Point.ToString();
        }
    }
}
