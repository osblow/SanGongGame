using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.App;

namespace Osblow.Game
{
    public class SingleCard : MonoBehaviour
    {
        public bool IsBigSize = false;

        [Space]
        public RawImage Bg;
        public RawImage Num;
        public RawImage Suit_small;
        public RawImage Suit_Big;


        private const string c_cardsRootPath = "Texture/cards_new/";

        public uint CurNum = 0;

        public void SetNum(uint cardVal)
        {
            CurNum = cardVal;
            

            if (IsBigSize)
            {
                //Num.gameObject.SetActive(false);
                //Suit_Big.gameObject.SetActive(false);
                //Suit_small.gameObject.SetActive(false);

                Bg.texture = Globals.SceneSingleton<CardMng>().GetSprite(cardVal);
            }
            else if(cardVal == 54)
            {
                Num.gameObject.SetActive(false);
                Suit_Big.gameObject.SetActive(false);
                Suit_small.gameObject.SetActive(false);

                Bg.texture = Globals.SceneSingleton<CardMng>().GetSprite(cardVal);
            }
            else
            {
                Bg.texture = Resources.Load<Texture>(c_cardsRootPath + "front");
                Num.gameObject.SetActive(true);
                Suit_Big.gameObject.SetActive(true);
                Suit_small.gameObject.SetActive(true);

                int numVal = ((int)cardVal % 13) + 1;
                int suit = (int)cardVal / 13;

                SetNum(suit, numVal);
                SetSuit(suit);
            }
        }

        
        private void SetNum(int suit, int numVal)
        {
            string path = "";

            string numSign = GetNumString(numVal);
            

            if(suit == 0 || suit == 2)
            {
                path = c_cardsRootPath + numSign + numSign;
            }
            else
            {
                path = c_cardsRootPath + numSign + "_" + numSign;
            }

            Num.texture = Resources.Load<Texture>(path);
        }
        
        string GetNumString(int numVal)
        {
            if (numVal == 11)
            {
                return "J";
            }
            else if (numVal == 12)
            {
                return "Q";
            }
            else if (numVal == 13)
            {
                return "K";
            }
            else if (numVal == 1)
            {
                return "A";
            }
            else
            {
                return numVal.ToString();
            }
        }

        private void SetSuit(int suit)
        {
            string path = "";
            Texture theSprite = null;

            switch (suit)
            {
                case 0:
                    path = c_cardsRootPath + "spade";
                    break;
                case 1:
                    path = c_cardsRootPath + "heart";
                    break;
                case 2:
                    path = c_cardsRootPath + "club";
                    break;
                case 3:
                    path = c_cardsRootPath + "diamond";
                    break;
            }


            theSprite = Resources.Load<Texture>(path);
            Suit_small.texture = theSprite;
            Suit_Big.texture = theSprite;
        }


        //private void Start()
        //{
        //    SetNum(9);
        //}
    }
}
