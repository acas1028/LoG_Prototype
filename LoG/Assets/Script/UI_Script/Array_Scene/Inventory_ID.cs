using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_ID : MonoBehaviour//,IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    public int inventory_ID;
    public int inventory_Num; // inventory id�� �κ��丮 ��ȣ�� ����Ű�°� �ƴ϶� character id�� ����Ű�� ���������, ���ڸ� ����Ű�� ������ �����Ͽ����ϴ�.
    public GameObject Block_Inventory;
    public bool is_Arrayed;

    private void Update()
    {

    }
    public void Setting_Character_Stat(GameObject Popup)
    {
        Popup.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(inventory_Num);
    }
    public void SetArrayed()
    {
        is_Arrayed = true;
        Block_Inventory.SetActive(true);
    }
    public void SetNotArrayed()
    {
        is_Arrayed = false;
        Block_Inventory.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }

}
