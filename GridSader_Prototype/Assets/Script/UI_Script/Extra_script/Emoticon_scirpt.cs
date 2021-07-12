using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Emoticon_scirpt : MonoBehaviour
{
    GameObject emoticon_popup;

    bool is_Close;
    bool is_Open;


    private void Start()
    {
        is_Open = false;
        is_Close = false;
        emoticon_popup = GameObject.FindGameObjectWithTag("Emoticon_Popup");
        emoticon_popup.SetActive(false);
    }

    private void Update()
    {

    }

    public void Emoticon_Popup_On()
    {
        if (emoticon_popup.activeSelf == false)
        {
            emoticon_popup.SetActive(true);
            is_Open = true;
        }
        else if(emoticon_popup.activeSelf== true && is_Close==false)
        {
            is_Close = true;
            
        }

    }



}
