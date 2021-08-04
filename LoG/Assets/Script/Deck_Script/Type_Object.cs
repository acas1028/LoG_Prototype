using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Type_Object : MonoBehaviour,IPointerClickHandler
{
    public enum Type_Num
    {
        Attacker = 1,
        Defender,
        Balance
    };
    public Type_Num Type;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Deck_Manager cs = Deck_Manager.instance;

        int j = 0;
        while (j < 7)
        {
            if (cs.Current_Character==cs.Character_Slot[j])
            {
                cs.Slot_Type[j].GetComponent<Deck_Type_Slot>().Change_Type((int)Type);
                break;
            }
            j++;
        }
    }
}
