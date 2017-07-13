using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Manage Context For UI Stack
 *
 *	by Xuanyi
 *
 */

namespace Osblow.App
{
    public class ContextManager : MonoBehaviour
    {
        private Stack<BaseContext> _contextStack = new Stack<BaseContext>();

        private void Awake()
        {
            //Push(new MainMenuContext());
        }

        public void Push(BaseContext nextContext)
        {

            if (_contextStack.Count != 0)
            {
                BaseContext curContext = _contextStack.Peek();
                GameObject curViewObj = Globals.SceneSingleton<UIManager>().GetSingleUI(curContext.ViewType);

                if (curViewObj)
                {
                    BaseView curView = curViewObj.GetComponent<BaseView>();
                    if (curView != null) curView.OnPause(curContext);
                }
            }

            _contextStack.Push(nextContext);
            BaseView nextView = Globals.SceneSingleton<UIManager>().GetSingleUI(nextContext.ViewType, true).GetComponent<BaseView>();
            nextView.OnEnter(nextContext);
        }

        public void Pop()
        {
            if (_contextStack.Count != 0)
            {
                BaseContext curContext = _contextStack.Peek();
                _contextStack.Pop();

                GameObject curViewObj = Globals.SceneSingleton<UIManager>().GetSingleUI(curContext.ViewType);

                if (curViewObj)
                {
                    BaseView curView = curViewObj.GetComponent<BaseView>();
                    if (curView != null) curView.OnExit(curContext);
                }
            }

            if (_contextStack.Count != 0)
            {
                BaseContext lastContext = _contextStack.Peek();
                BaseView curView = Globals.SceneSingleton<UIManager>().GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
                curView.OnResume(lastContext);
            }
        }

        public BaseContext PeekOrNull()
        {
            if (_contextStack.Count != 0)
            {
                return _contextStack.Peek();
            }
            return null;
        }
        
        public void WebBlockUI(bool isBlock)
        {
            BaseContext curContext = PeekOrNull();
            if (isBlock)
            {
                if(curContext != null && curContext is LoadingUIContext)
                {
                    return;
                }

                Push(new LoadingUIContext());
            }
            else
            {
                if (curContext == null || !(curContext is LoadingUIContext))
                {
                    return;
                }

                Pop();
            }
        }
    }
}
