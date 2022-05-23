using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveComplete : MonoBehaviour
{
    public Deck_Manager deck_Manager;

    public GameObject saving_Popup;

    public void DeckCompleteSave()
    {
        for(int i =0; i< 7; i++)
        {
            if(deck_Manager.Character_Slot[i].GetComponentInChildren<Character>().character_ID == 0)
            {
                return;
            }
        }

        StartCoroutine(CompleteSavePopup());

    }

    IEnumerator CompleteSavePopup()
    {
        saving_Popup.SetActive(true);

        yield return new WaitForSeconds(2f);

        saving_Popup.SetActive(false);
    }

    
}
