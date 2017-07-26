using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CuoPaiZhong : MonoBehaviour 
{
    public RectTransform Cuo;
    public RectTransform Pai;
    public RectTransform Zhong;

    private Vector3 m_cuoOriginPos;
    private Vector3 m_paiOriginPos;
    private Vector3 m_zhongOriginPos;

    private bool m_isInitialized = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        m_cuoOriginPos = Cuo.position;
        m_paiOriginPos = Pai.position;
        m_zhongOriginPos = Zhong.position;

        OnDisable();

        m_animCoroutine = StartCoroutine(Anim());

        m_isInitialized = true;
    }

    private Coroutine m_animCoroutine = null;
    private void OnEnable()
    {
        if (!m_isInitialized)
        {
            return;
        }

        OnDisable();

        m_animCoroutine = StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        while (true)
        {
            Cuo.position = m_cuoOriginPos;
            Cuo.DOMove(m_cuoOriginPos + Vector3.up * 20, 0.3f).SetLoops(2, LoopType.Yoyo);

            Pai.position = m_paiOriginPos;
            Pai.DOMove(m_paiOriginPos + Vector3.up * 20, 0.3f).SetLoops(2, LoopType.Yoyo).SetDelay(0.15f);

            Zhong.position = m_zhongOriginPos;
            Zhong.DOMove(m_zhongOriginPos + Vector3.up * 20, 0.3f).SetLoops(2, LoopType.Yoyo).SetDelay(0.3f);

            yield return new WaitForSeconds(1.5f);
        }
    }

    private void OnDisable()
    {
        if(null == m_animCoroutine)
        {
            return;
        }

        StopCoroutine(m_animCoroutine);
    }
}
