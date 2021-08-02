using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property_Button : MonoBehaviour
{
    public GameObject[] property_Popup;

    public void Property_Button_Click(GameObject present_Property_Popup)
    {
        present_Property_Popup.SetActive(true);

        for(int i=0; i<property_Popup.Length;i++)
        {
            if(property_Popup[i]!=present_Property_Popup)
            {
                property_Popup[i].SetActive(false);
            }
        }
    }
}
