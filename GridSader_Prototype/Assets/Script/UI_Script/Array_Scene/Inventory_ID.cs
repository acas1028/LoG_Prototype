using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory_ID : MonoBehaviour
{
    public int m_Inventory_ID;
    public int m_Character_ID;
    public GameObject Block_Inventory;
    public bool is_Arrayed;
   

    private void Update()
    {
        if(is_Arrayed==true)
        {
            Block_Inventory.SetActive(true); 
        }
        else
        {
            Block_Inventory.SetActive(false);
        }

    }
    public void Setting_Character_Stat(GameObject Popup)
    {
        Popup.GetComponent<ShowingCharacterStats>().Character_Showing_Stats(m_Character_ID);
    }
    public void Save_Character_ID()
    {
        Arrayment_Manager.Array_instance.Get_Button(m_Character_ID);
    }

    

}
