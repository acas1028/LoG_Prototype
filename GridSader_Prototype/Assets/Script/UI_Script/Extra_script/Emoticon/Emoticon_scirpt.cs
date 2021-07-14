using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Emoticon_scirpt : MonoBehaviour
{
    GameObject emoticon_popup; //이모티콘 내용이 담긴 popup
    bool is_Open; //pop이 켜져있는가를 구별하는 bool 변수


    private void Start()
    {
        
        is_Open = false;
        emoticon_popup = GameObject.FindGameObjectWithTag("Emoticon_Popup");
        emoticon_popup.SetActive(false);        
    }

    private void Update()
    {
        Emoticon_Animation();
    }


    public void Emoticon_Popup_On() //이모티콘의 on off 함수
    {
        if (emoticon_popup.activeSelf == false)
        {
            emoticon_popup.SetActive(true);
            is_Open = true;
        }
        
        else
        {
            is_Open = false;
        }

    }

    void Emoticon_Animation() //이모티콘 popup창 애니메이션 관리 함수
    {
        if(is_Open==false)
        {
            emoticon_popup.GetComponent<Animator>().SetInteger("Emoticon_Turn", 1);

            if (emoticon_popup.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EmoticonPopup_Close"))
            {
                if (emoticon_popup.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    emoticon_popup.SetActive(false);
                }
            }

        }

        
    }




}
