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
        Deck_Manager.instance.Current_Grid = EventSystem.current.currentSelectedGameObject;
    }
}
