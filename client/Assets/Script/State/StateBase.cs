﻿using System;
using System.Collections;
using System.Collections.Generic;


namespace Osblow.App
{
    public enum StateType
    {
        None = 0,
        Login = 1,
        Lobby = 2,
        Game = 3,
        GameResult = 4
    }

    public abstract class StateBase
    {
        public abstract StateType StateType { get; }

        protected StateMng m_parentMng;

        public StateBase(StateMng parent)
        {
            m_parentMng = parent;
        }


        public virtual void Begin(StateType lastState) { }

        public virtual void Update(float deltaTime) { }

        public virtual void End(StateType nextState) { }
    }

    public class LoginState : StateBase
    {
        public override StateType StateType
        {
            get
            {
                return StateType.Login;
            }
        }

        public LoginState(StateMng parent) : base(parent) { }


        public override void Begin(StateType lastState)
        {
            base.Begin(lastState);
            Globals.SceneSingleton<ContextManager>().Push(new LoginUIContext());
        }
    }

    public class LobbyState : StateBase
    {
        public override StateType StateType
        {
            get
            {
                return StateType.Lobby;
            }
        }

        public LobbyState(StateMng parent) : base(parent) { }
    }

    public class GameState : StateBase
    {
        public override StateType StateType
        {
            get
            {
                return StateType.Game;
            }
        }

        public GameState(StateMng parent) : base(parent) { }
    }

    public class GameResultState : StateBase
    {
        public override StateType StateType
        {
            get
            {
                return StateType.GameResult;
            }
        }

        public GameResultState(StateMng parent) : base(parent) { }
    }
}