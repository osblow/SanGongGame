using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 因为只有主线程可访问场景资源，异步调用全部在这里注册，然后帧末执行
/// </summary>
public class AsyncInvokeMng : MonoBehaviour
{
    public event Action EventsToAct;


    public void LateUpdate()
    {
        if (null != EventsToAct)
        {
            EventsToAct.Invoke();
            EventsToAct = null;
        }
    }
}
