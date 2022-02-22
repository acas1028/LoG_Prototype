using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck_Page : MonoBehaviour
{
    public Deck_Manager deckManager;
    public DeckDataSync deckDataSync;
    public int pageIdx;
    private Image Button_image;

    public void Click_Deck()
    {
        Button_image = gameObject.GetComponent<Image>();
        deckManager.SetNowPageIndex(pageIdx);
        deckManager.Switch_Page();

        Button_image.GetComponent<Image>().color = new Color(117, 255, 0);
        for (int i = 0; i < 5; i++)
        {
            if (i != pageIdx)
            {
                deckManager.Page_Slot[i].GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }

        for (int i = 0; i < deckManager.CharacterSpace.Length; i++)
        {
            deckManager.CharacterSpace[i].GetComponent<ShowSprite_SaveDeckData>().Show_Type_Sprite();
        }

        for(int i = 0; i<deckManager.Property_Slot.Length;i++)
        {
            if(deckManager.Property_Slot[i].transform.childCount ==2)
            {
                deckManager.Property_Slot[i].SetActive(false);
            }
        }
    }
}
