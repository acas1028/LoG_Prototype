using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_FlowScript : MonoBehaviour
{
    //밑에 주석처리 되어있는 변수나 함수는 현재 아직 보류중인 내용들이기에 남겨둔 것입니다.

    public Text time_text; //시간을 띄어주는 텍스트
    int text_in_time; //현재 텍스트에 출력된 시간
    int max_time; // 최대시간 =ex) 30초
    float time;  // 1초마다 1씩 줄이기 위해 만든, time.deltatime을 더하기 위해 만든 시간 변수
    //float blink_time;
    //bool is_Blink;
    //bool is_fast_Blink;

    private void Start()
    {
        max_time = 30;
        text_in_time = max_time;
        time = 0;
        //is_Blink = false;
        //is_fast_Blink = false;
        time_text.text = text_in_time.ToString();
    }

    private void Update()
    {
        Time_on();
        TimeFlow();
        Time_over();

        //if(text_in_time<10)
        //{
        //    Blink_TimeFlow();
        //    Blink_time();
        //    Blink();
        //}

        if(Input.GetKeyDown(KeyCode.W)) // 임시
        {
            Time_reset();
        }

    }

    void Time_on() //1초마다 시간이 줄어든다.(텍스트 출력까지)
    {
        if(time>=1)
        {
            text_in_time--;
            time_text.text = text_in_time.ToString();
            time = 0;
        }

        
    }

    void TimeFlow() //1초라는 시간을 재기 위한 time의 현실 시간 흐름을 위한 함수
    {
        time += Time.deltaTime;
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

    void Time_reset() //시간 max_Time으로 리셋
    {
        text_in_time = max_time;
        time = 0;
    }

    void Time_over() //시간이 0초 이하가 되었을 때의 함수
    {
        if (text_in_time < 0)
        {
            Debug.Log("타임 오버");
        }
    }
}
