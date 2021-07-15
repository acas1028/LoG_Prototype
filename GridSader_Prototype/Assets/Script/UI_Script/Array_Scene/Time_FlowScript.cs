using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class Time_FlowScript : MonoBehaviour
{
    public bool Time_Over;

    [SerializeField]
    float time;

    int max_Time;

    private void Start()
    {
        max_Time = 30;
        time = max_Time;
        Time_Over = false;
    }

    private void OnEnable()
    {
        max_Time = 30;
        time = max_Time;
        Time_Over = false;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        this.GetComponent<Text>().text = ((int)time).ToString();

        if (time <= 0)
            Time_over();

        if(Input.GetKeyDown(KeyCode.W)) // 임시
        {
            Time_reset();
        }
    }

    void Time_reset() //시간 max_Time으로 리셋
    {
        time = max_Time;
    }

    void Time_over() //시간이 0초 이하가 되었을 때의 함수
    {
        Time_Over = true;
        this.gameObject.SetActive(false);
    }

    //void Blink_TimeFlow()
    //{
    //    blink_time += Time.deltaTime;
    //}

    //void Blink_time()
    //{
    //    if(text_in_time<=10 && text_in_time>5)
    //    {
    //        is_Blink = true;
    //    }
    //}

    //void Blink()
    //{
    //    if(is_Blink ==true)
    //    {
    //        if(blink_time>=0.25)
    //        {
    //            if(time_text.gameObject.activeSelf==true)
    //            {
    //                time_text.gameObject.SetActive(false);
    //                blink_time = 0;
    //            }

    //            else
    //            {
    //                time_text.gameObject.SetActive(true);
    //                blink_time = 0;
    //            }
    //        }
    //    }
    //}
}
