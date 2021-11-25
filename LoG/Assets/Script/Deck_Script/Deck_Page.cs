using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck_Page : MonoBehaviour
{
    public GameObject[] characterData;
    public DeckDataSync deckDataSync;
    public int P_num;
    private Image Button_image;


    private void Start()
    {
        characterData = GameObject.FindGameObjectsWithTag("DeckCharacterData");
    }
    public void Click_Deck()
    {
        Deck_Manager DM = Deck_Manager.instance;

        P_num = this.gameObject.GetComponent<Deck_Page>().P_num;
        Button_image = this.gameObject.GetComponent<Image>();
        DM.Page_Num = P_num;

        //deckDataSync.GetData(P_num);
        for (int i = 0; i < Deck_Manager.instance.Character_Slot.Length; i++)
        {
            Deck_Manager.instance.Character_Slot[i].GetComponentInChildren<Character>().character_ID = 0;

        }
        DM.StartCoroutine("Switch_Page");

        Button_image.GetComponent<Image>().color = new Color(117, 255, 0);
        for (int i = 0; i < 5; i++)
        {
            if (i != P_num)
            {
                DM.Page_Slot[i].GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }



        for (int i = 0; i < characterData.Length; i++)
        {
            characterData[i].GetComponent<ShowSprite_SaveDeckData>().Show_Type_Sprite();
        }
    }
}
