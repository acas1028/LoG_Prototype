using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_ID : MonoBehaviour//,IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    public int m_Inventory_ID;
    public GameObject Block_Inventory;
    public bool is_Arrayed;
   

    private void Update()
    {

    }
    public void Setting_Character_Stat(GameObject Popup)
    {
        //Popup.GetComponent<ShowingCharacterStats>().Character_Showing_Stats();
    }
    public void Click_Inven()
    {
        Inventory_ID cs = this.gameObject.GetComponent<Inventory_ID>();
        cs.is_Arrayed = true;
        cs.Block_Inventory.SetActive(true);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }

}
