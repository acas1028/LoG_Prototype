using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Property_sending_to_slot : MonoBehaviour, IPointerClickHandler
{
    public string property_Name;

    public void OnPointerClick(PointerEventData eventData)
    {
        Deck_Manager cs = Deck_Manager.instance;

        int j = 0;
        while (j < 7)
        {
            if (cs.Current_Character == cs.Character_Slot[j])
            {
                cs.Slot_Property[j].GetComponent<Property_Slot>().Change_property(property_Name);
                break;
            }
            j++;
        }
    }
}
