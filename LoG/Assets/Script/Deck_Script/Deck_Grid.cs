using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck_Grid : MonoBehaviour
{
    public bool is_Clicked_Grid = false;
    public int Grid_Num;
    public void Click_Grid_1()
    {
        Deck_Grid cs = this.gameObject.GetComponent<Deck_Grid>();
        Deck_Manager.instance.Current_Grid = EventSystem.current.currentSelectedGameObject;
        if(cs.is_Clicked_Grid==false)
            cs.is_Clicked_Grid = true;
        else
        {
            cs.is_Clicked_Grid = false;
        }
    }
}
