using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Emoticon_scirpt : MonoBehaviour
{
    GameObject emoticon_popup;
    bool is_Open;


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


    public void Emoticon_Popup_On()
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

    void Emoticon_Animation()
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
