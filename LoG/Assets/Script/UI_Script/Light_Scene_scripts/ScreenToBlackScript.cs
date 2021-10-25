using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToBlackScript : MonoBehaviour
{
    public GameObject settingPopup;

    public void SettingPopupOff()
    {
        if (this.gameObject.activeSelf == true)
        {
            settingPopup.GetComponent<animation_On_off>().AnimationOff();
            this.gameObject.SetActive(false);
        }
    }
}
