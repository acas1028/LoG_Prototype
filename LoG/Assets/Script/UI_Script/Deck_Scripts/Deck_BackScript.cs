using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck_BackScript : MonoBehaviour
{
    [SerializeField]
    GameObject backPopup;

    [SerializeField]
    Deck_Manager deckmanager;

    void Update()
    {

        for (int i = 0; i < deckmanager.Character_Slot.Length; i++)
        {
            if (deckmanager.Character_Slot[i].GetComponentInChildren<Character>().character_ID == 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        backPopup.SetActive(true);
                    }
                }
            }
        }
    }

}
