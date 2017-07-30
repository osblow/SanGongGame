using UnityEngine;
using UnityEngine.UI;

public class VoiceAnim : MonoBehaviour 
{
    private Image m_effect;

    public float[] Ratios;
    private int m_curIndex = 0;

    public float Speed;

    private float m_timer;



    private void Start()
    {
        m_effect = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update () 
	{
        m_timer += Time.deltaTime * Speed;
		if(m_timer < 1.0f)
        {
            return;
        }
        m_timer = 0;

        m_effect.fillAmount = Ratios[m_curIndex % Ratios.Length];
        ++m_curIndex;
	}
}
