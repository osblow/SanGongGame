using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Osblow.App
{
    public class CardMng : MonoBehaviour
    {
        private const string c_cardTexPath = "Texture/Cards/";
        private List<Sprite> m_cardTextures = new List<Sprite>();


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Sprite GetSprite(uint cardVal)
        {
            if (m_cardTextures.Count <= 0)
            {
                InitCardSprites();
            }

            if (cardVal < 0 || cardVal > 54)
            {
                Debug.Log("牌面值不合法： " + cardVal);
                return null;
            }

            return m_cardTextures[(int)cardVal];
        }

        public Sprite GetBackSprite()
        {
            if (m_cardTextures.Count <= 0)
            {
                InitCardSprites();
            }

            return m_cardTextures[54];
        }

        void InitCardSprites()
        {
            for (int i = 1; i < 54; i++)
            {
                Sprite temp = Resources.Load<Sprite>(c_cardTexPath + i);
                m_cardTextures.Add(temp);
            }

            m_cardTextures.Add(Resources.Load<Sprite>(c_cardTexPath + "back"));
        }
    }
}