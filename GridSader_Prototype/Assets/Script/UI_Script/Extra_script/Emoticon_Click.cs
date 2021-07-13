using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Emoticon_Click : MonoBehaviour
{
    GameObject emoticon_Block;
    GameObject emoticon_Manager;
    GameObject emoticon_Time_Manager;

    private void Start()
    {
        emoticon_Manager = GameObject.FindGameObjectWithTag("Emoticon_Manager");
        emoticon_Time_Manager = emoticon_Manager.transform.GetChild(0).gameObject;
        emoticon_Block = emoticon_Time_Manager.GetComponent<Emoticon_Time_Script>().get_Emoticon_Block();
    }

    public void Emoticon_click()
    {
        
            emoticon_Block.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            emoticon_Time_Manager.GetComponent<Emoticon_Time_Script>().Plus_Emoticon_open_Count(1);
            emoticon_Time_Manager.GetComponent<Emoticon_Time_Script>().Set_Time(0);
        
        
    }
}
