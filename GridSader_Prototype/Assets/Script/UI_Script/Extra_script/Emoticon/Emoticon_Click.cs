using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Emoticon_Click : MonoBehaviour
{
    GameObject emoticon_Block; //이모티콘이 나타나는 위치
    GameObject emoticon_Manager; //이모티콘 매니저(관리)
    GameObject emoticon_Time_Manager;//이모티콘에 관한 시간 관련을 전부 관리해주는 매니저

    private void Start()
    {
        emoticon_Manager = GameObject.FindGameObjectWithTag("Emoticon_Manager");
        emoticon_Time_Manager = emoticon_Manager.transform.GetChild(0).gameObject;
        emoticon_Block = emoticon_Time_Manager.GetComponent<Emoticon_Time_Script>().get_Emoticon_Block();
    }

    public void Emoticon_click() //이모티콘 클릭시 구현되는 함수
    {
        if (emoticon_Time_Manager.GetComponent<Emoticon_Time_Script>().Get_Is_Stop_Emoticon() == false)
        {
            emoticon_Block.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            emoticon_Time_Manager.GetComponent<Emoticon_Time_Script>().Plus_Emoticon_open_Count(1);
            emoticon_Time_Manager.GetComponent<Emoticon_Time_Script>().Set_Time(0);
        }
        
        
    }
}
