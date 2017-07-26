using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Osblow.App
{
    public class StateMng : MonoBehaviour
    {
        private Dictionary<StateType, StateBase> m_stateDic =
            new Dictionary<StateType, StateBase>();
        private StateBase m_curState;
        private StateType m_curStateType = StateType.None;

        private void Awake()
        {
            m_stateDic.Add(StateType.Login, new LoginState(this));
            m_stateDic.Add(StateType.Lobby, new LobbyState(this));
            m_stateDic.Add(StateType.Game, new GameState(this));
            m_stateDic.Add(StateType.GameResult, new GameResultState(this));
        }


        public void ChangeState(StateType nextStateType)
        {
            if (!m_stateDic.ContainsKey(nextStateType) ||
                m_curStateType == nextStateType)
            {
                return;
            }

            
            if(m_curState != null)
            {
                m_curState.End(nextStateType);
            }

            StateBase nextState = m_stateDic[nextStateType];
            nextState.Begin(m_curStateType);

            m_curState = nextState;
            m_curStateType = nextStateType;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
