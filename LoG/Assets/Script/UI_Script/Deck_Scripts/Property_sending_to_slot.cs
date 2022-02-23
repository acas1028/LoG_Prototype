using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Property_sending_to_slot : MonoBehaviour, IPointerClickHandler
{
    public string property_Name;
    Deck_Manager deckManager;

    private void Awake()
    {
        deckManager = FindObjectOfType<Deck_Manager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int j = 0;
        
        if (this.GetComponent<Deck_Skill>().is_selected == true)
        {
           
            while (j < 7)
            {
                if (deckManager.Current_Character == deckManager.Character_Slot[j])
                {
                    deckManager.Slot_Property[j].GetComponent<Property_Slot>().Change_property(property_Name);
                    break;
                }
                j++;
            }
        }

    }
}
