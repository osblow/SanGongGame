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
            gameObject.SetActive(true);
            transform.SetAsLastSibling();

            UIAnimationBase anim = transform.GetComponent<UIAnimationBase>();
            if (anim)
            {
                anim.Anim();
            }
        }

        public virtual void OnExit(BaseContext context)
        {
            gameObject.SetActive(false);
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
