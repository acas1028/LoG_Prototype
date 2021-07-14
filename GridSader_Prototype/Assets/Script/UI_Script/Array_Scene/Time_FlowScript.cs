using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_FlowScript : MonoBehaviour
{
    //�ؿ� �ּ�ó�� �Ǿ��ִ� ������ �Լ��� ���� ���� �������� ������̱⿡ ���ܵ� ���Դϴ�.

    public Text time_text; //�ð��� ����ִ� �ؽ�Ʈ
    int text_in_time; //���� �ؽ�Ʈ�� ��µ� �ð�
    int max_time; // �ִ�ð� =ex) 30��
    float time;  // 1�ʸ��� 1�� ���̱� ���� ����, time.deltatime�� ���ϱ� ���� ���� �ð� ����
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

        if(Input.GetKeyDown(KeyCode.W)) // �ӽ�
        {
            Time_reset();
        }

    }

    void Time_on() //1�ʸ��� �ð��� �پ���.(�ؽ�Ʈ ��±���)
    {
        if(time>=1)
        {
            text_in_time--;
            time_text.text = text_in_time.ToString();
            time = 0;
        }

        
    }

    void TimeFlow() //1�ʶ�� �ð��� ��� ���� time�� ���� �ð� �帧�� ���� �Լ�
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

    void Time_reset() //�ð� max_Time���� ����
    {
        text_in_time = max_time;
        time = 0;
    }

    void Time_over() //�ð��� 0�� ���ϰ� �Ǿ��� ���� �Լ�
    {
        if (text_in_time < 0)
        {
            Debug.Log("Ÿ�� ����");
        }
    }
}
