using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Manager : MonoBehaviour
{
    GameObject Setting_Popup;
    bool is_Open;

    private void Start()
    {
        is_Open = false;
        Setting_Popup = GameObject.FindGameObjectWithTag("Setting_Popup");
        Setting_Popup.SetActive(false);
    }

    private void Update()
    {
        Setting_Popup_Animaition();
    }

    public void Setting_Popup_On() //환경 설정 on off 함수
    {
        if(Setting_Popup.activeSelf==false)
        {
            Setting_Popup.SetActive(true);
            is_Open = true;
        }

        else
        {
            is_Open = false;
        }
    }

    void Setting_Popup_Animaition()//환경설정 popup창 애니메이션 관리 함수
    {
        if(is_Open==false)
        {
            Setting_Popup.GetComponent<Animator>().SetInteger("SettingPopup_Turn", 1);

            if(Setting_Popup.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SettingPopup_Close"))
            {
                if(Setting_Popup.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
                {
                    Setting_Popup.SetActive(false);
                }

            }
        }
    }
}
