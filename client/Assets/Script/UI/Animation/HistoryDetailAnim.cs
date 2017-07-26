using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryDetailAnim : MonoBehaviour 
{
    public float GridCellHeight;
    public float MoveDist;
    public List<GameObject> Items;

    private Vector3 m_topItemPos;
    private Queue<AnimAction> m_actionsQueue = new Queue<AnimAction>();
    private List<bool> m_savedAnimState = new List<bool>();


    public void SetAnim(int index)
    {
        if(index <0||index >= Items.Count)
        {
            return;
        }

        m_savedAnimState[index] = !m_savedAnimState[index];

        foreach (AnimAction ac in m_actionsQueue)
        {
            ac.InOrOut = true;
            m_savedAnimState[ac.Index] = true;
        }

        int tempIndex = 0;
        m_savedAnimState.ForEach((x) => Debug.Log(++tempIndex + " " + x));

        

        AnimAction newAction = new AnimAction();
        newAction.Index = index;
        newAction.OriginPos = m_topItemPos - Vector3.up * GridCellHeight * index;
        newAction.TargetPos = newAction.OriginPos - Vector3.up * MoveDist;
        newAction.InOrOut = m_savedAnimState[index];

        m_actionsQueue.Enqueue(newAction);

        StartAnim();
    }

    Coroutine m_animCoroutine = null;
#if UNITY_EDITOR
    const float c_animStep = 10;
#else
    const float c_animStep = 40;
#endif
    IEnumerator AnimImp()
    {
        int curIndex = 0;
        while (curIndex < m_actionsQueue.Count)
        {
            AnimAction theAction = m_actionsQueue.Peek();
            Vector3 target = theAction.InOrOut ? theAction.OriginPos : theAction.TargetPos;
            
            while (Mathf.Abs(Items[theAction.Index].transform.localPosition.y - target.y) >= 1)
            {
                Vector3 lastPos = Items[theAction.Index].transform.localPosition;
                Items[theAction.Index].transform.localPosition = Vector3.MoveTowards(Items[theAction.Index].transform.localPosition, target, c_animStep);
                Vector3 offset = Items[theAction.Index].transform.localPosition - lastPos;
                for (int i = theAction.Index + 1; i < Items.Count; i++)
                {
                    Items[i].transform.localPosition += offset;
                }
                yield return null;
            }

            if (theAction.InOrOut)
            {
                m_actionsQueue.Dequeue();
                --curIndex;
            }

            ++curIndex;
        }
    }

    void StartAnim()
    {
        Stop();
        m_animCoroutine = StartCoroutine(AnimImp());
    }

    void Stop()
    {
        if(null != m_animCoroutine)
        {
            StopCoroutine(m_animCoroutine);
        }
    }

	// Use this for initialization
	void Start () 
	{
        if(Items.Count <= 0)
        {
            return;
        }

        m_topItemPos = Items[0].transform.localPosition;
        for (int i = 0; i < Items.Count; i++)
        {
            if (i >= m_savedAnimState.Count)
            {
                m_savedAnimState.Add(true);
            }
            else
            {
                m_savedAnimState[i] = true;
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}









    class AnimAction
    {
        public int Index = 0;
        public Vector3 OriginPos;
        public Vector3 TargetPos;

        public bool InOrOut = false;
    }
}
