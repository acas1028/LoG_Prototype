using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_ID : MonoBehaviour//,IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    public int m_Inventory_ID;
    public int m_Character_ID;
    public GameObject Block_Inventory;
    public bool is_Arrayed;
   

    private void Update()
    {

    }
    public void Setting_Character_Stat(GameObject Popup)
    {
        Popup.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(m_Character_ID);
    }
    public int Return_Character_ID()
    {
        return this.gameObject.GetComponent<Inventory_ID>().m_Character_ID;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }

}
