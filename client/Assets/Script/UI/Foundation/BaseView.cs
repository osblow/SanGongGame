using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Base View
 *
 *	by Xuanyi
 *
 */

namespace Osblow.App
{
	public abstract class BaseView : MonoBehaviour
    {

        public virtual void OnEnter(BaseContext context)
        {
            transform.SetAsLastSibling();
        }

        public virtual void OnExit(BaseContext context)
        {

        }

        public virtual void OnPause(BaseContext context)
        {

        }

        public virtual void OnResume(BaseContext context)
        {

        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
	}
}
