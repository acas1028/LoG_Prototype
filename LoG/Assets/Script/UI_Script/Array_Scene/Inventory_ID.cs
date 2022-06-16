using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_ID : MonoBehaviour
{
    [SerializeField]
    private int CharacterID;
    [SerializeField]
    public int inventory_Num;
    [SerializeField]
    private GameObject popUp;
    public GameObject Block_Inventory;

    public bool is_Arrayed;
    public bool is_Permanent_Arrayed;

    private void Start()
    {
        Setting_Character_Stat(popUp);
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
    public void SetPermanentArrayed() {
        is_Arrayed = true;
        is_Permanent_Arrayed = true;
        Block_Inventory.SetActive(true);
    }
    public void SetNotArrayed()
    {
        if (!is_Permanent_Arrayed) {
            is_Arrayed = false;
            Block_Inventory.SetActive(false);
        }
    }
    public int GetCharacterID()
    {
        return this.CharacterID;
    }
    public void SetCharacterID(int num)
    {
        this.CharacterID = num;
    }
    public int GetInventoryNum()
    {
        return this.inventory_Num;
    }
    public void SetInventoryNum(int num)
    {
        this.inventory_Num = num;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }

}
