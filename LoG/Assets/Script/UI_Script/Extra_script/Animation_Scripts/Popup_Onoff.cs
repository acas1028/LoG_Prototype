using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Onoff : MonoBehaviour
{
    public GameObject popup;



    private void Start()
    {
        popup.SetActive(false); //аж╫ц 
    }
    public void PopupOnOff()
    {
        if (popup == null)
        {
            return;
        }

        if (popup.activeSelf==false)
        {
            popup.GetComponent<animation_On_off>().AnimationOn();
            Limit_Popup.Limit_Poup_instance.SetIsbutton(this.gameObject);
            Limit_Popup.Limit_Poup_instance.SetPopup(popup);
            Limit_Popup.Limit_Poup_instance.limit_Popup_button.SetActive(true);
            
        }

        else
        {
            if (Limit_Popup.Limit_Poup_instance.GetisButton() == this.gameObject)
            {
                popup.GetComponent<animation_On_off>().AnimationOff();
                Limit_Popup.Limit_Poup_instance.SetIsbutton(null);
                Limit_Popup.Limit_Poup_instance.limit_Popup_button.SetActive(false);
                Limit_Popup.Limit_Poup_instance.BoolPopupOn_Off();
            }
        }
    }


    
}
