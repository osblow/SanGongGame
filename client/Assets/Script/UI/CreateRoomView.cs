using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Game;


namespace Osblow.App
{
    public class CreateRoomUIContext : BaseContext
    {
        public CreateRoomUIContext() : base(UIType.CreateRoomView) { }
    }

    public class CreateRoomView : BaseView
    {
        #region 场景引用 
        public List<GameObject> ModePanels;
        public List<Image> ModeButtons;
        #endregion



        #region 场景事件
        public void SelectMode(int mode)
        {
            for(int i = 0; i < ModePanels.Count; i++)
            {
                ModePanels[i].SetActive(i == mode);
                ModeButtons[i].color = i == mode ? Color.white : Color.gray;
            }

            m_roomData.RoomRuleType = mode;
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnCreateBtn()
        {
            OnCreateRoom();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnSelectPayMethod(int option)
        {
            if(option == 0)
            {
                // 房主掏钱
            }else if(option == 1)
            {
                // AA
            }

            m_roomData.RoomCostRule = option;
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        //public void OnSelectSanGond(int option)
        //{
        //    if(option == 0) // 三公
        //    {
        //        Debug.Log("三公");
        //    }
        //    else if(option == 1) // 双公
        //    {
        //        Debug.Log("双公");
        //    }

        //    m_isSanGong = option;
        //}

        //public void OnSelectHasOwner(int option)
        //{
        //    if(option == 0) // 抢庄
        //    {
        //        Debug.Log("抢庄");
        //    }
        //    else // 不抢庄
        //    {
        //        Debug.Log("不抢庄");
        //    }

        //    m_hasOwner = option;
        //}

        public void OnSelectAllowJoin()
        {
            m_roomData.IsJoin = m_roomData.IsJoin ^ 1;
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void SelectTotalRound(int round)
        {
            m_roomData.RoomTotalRound = round.ToString();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void SelectPlayerCount(int count)
        {
            m_roomData.MaxPlayerCount = count;
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        //public void OnSelectBaseScore(int score)
        //{
        //    m_baseScore = score;
        //    Debug.Log("底分：" + m_baseScore);
        //}

        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        void OnCreateRoom()
        {
            TableData data = new TableData();
            data.HasOwner = m_hasOwner;
            data.SanGong = m_isSanGong;
            data.BaseScore = m_baseScore;
            

            //TableUIContext context = new TableUIContext();
            //context.TableData = data;
            //Globals.SceneSingleton<ContextManager>().Push(context);

            Globals.SceneSingleton<ContextManager>().WebBlockUI(true);
            //Globals.SceneSingleton<SocketNetworkMng>();
            HttpRequest.CreateRoomRequest();
        }




        private int m_isSanGong = 0; // 0三公 ，1双公
        private int m_hasOwner = 0; // 0抢庄，1不抢庄
        private int m_baseScore = 0; // 底分

        private RoomData m_roomData = null;



        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            m_roomData = Globals.SceneSingleton<DataMng>().GetData<RoomData>(DataType.Room);
            if(m_roomData == null)
            {
                m_roomData = new RoomData();
                Globals.SceneSingleton<DataMng>().SetData(DataType.Room, m_roomData);
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