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

        #endregion

        #region 场景事件
        public void OnCreateBtn()
        {
            OnCreateRoom();
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


        }

        public void OnSelectSanGond(int option)
        {
            if(option == 0) // 三公
            {
                Debug.Log("三公");
            }
            else if(option == 1) // 双公
            {
                Debug.Log("双公");
            }

            m_isSanGong = option;
        }

        public void OnSelectHasOwner(int option)
        {
            if(option == 0) // 抢庄
            {
                Debug.Log("抢庄");
            }
            else // 不抢庄
            {
                Debug.Log("不抢庄");
            }

            m_hasOwner = option;
        }

        public void OnSelectBaseScore(int score)
        {
            m_baseScore = score;
            Debug.Log("底分：" + m_baseScore);
        }

        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
        }
        #endregion
        void OnCreateRoom()
        {
            TableData data = new TableData();
            data.HasOwner = m_hasOwner;
            data.SanGong = m_isSanGong;
            data.BaseScore = m_baseScore;

            RoomData roomData = new RoomData();
            roomData.RoomRuleName = "三公";
            Globals.SceneSingleton<DataMng>().SetData(DataType.Room, roomData);

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