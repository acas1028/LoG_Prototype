using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Onoff : MonoBehaviour
{
    public GameObject popup;


    private void Start()
    {
        popup.SetActive(false);
    }
    public void PopupOnOff()
    {
        if(popup== null)
        {
            return;
        }

        if(popup.activeSelf==false)
        {
            popup.GetComponent<animation_On_off>().AnimationOn();
        }

        else
        {
            popup.GetComponent<animation_On_off>().AnimationOff();
        }
    }
}
