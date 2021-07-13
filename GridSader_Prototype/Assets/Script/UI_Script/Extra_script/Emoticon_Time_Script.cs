using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emoticon_Time_Script : MonoBehaviour
{
    GameObject emoticonBlock;
    public float time;
    float animation_play_maxtime;
    int emoticon_open_Count; // count를 재어 present카운트와 다른 경우 애니메이션 작동
    int present_emoticon_open_Count; //현재의 카운트를 저장하는 용도
    bool is_emoticon_block_on;
    bool is_first_emoticon_in_repeat_time;
    bool is_stop_emoticon;

    public List<float> repeat_limit_list = new List<float>();

    private void Awake()
    {
        emoticonBlock = GameObject.FindGameObjectWithTag("Emoticon_Block");
        emoticonBlock.SetActive(false);
    }

    private void Start()
    {
        time = 0;
        animation_play_maxtime = 3;
        emoticon_open_Count = 0;
        present_emoticon_open_Count = 0;
        is_emoticon_block_on = false;
        is_first_emoticon_in_repeat_time = false;
        is_stop_emoticon = false;
    }

    private void Update()
    {
        Is_Animation_on();
        Is_Animation_multi_On();
        Animation_Time_Flow();
        Animation_Off();
        
    }

    void Is_Animation_on()
    {
        if(present_emoticon_open_Count != emoticon_open_Count && is_emoticon_block_on==false)
        {
            emoticonBlock.SetActive(true);
            is_emoticon_block_on = true;
            repeat_limit_list.Add(0);

        }

    }

    void Is_Animation_multi_On()
    {
        if (present_emoticon_open_Count != emoticon_open_Count && is_emoticon_block_on == true)
        {
            emoticonBlock.GetComponent<Animator>().Play("Emoticon_Click", -1, 0f);
            present_emoticon_open_Count = emoticon_open_Count;
            repeat_limit_list.Add(0);
        }
    }

    void Animation_Time_Flow()
    {
        if(is_emoticon_block_on==true)
        {
            time += Time.deltaTime;
        }
    }

    void Animation_Off()
    {
        if(time>=animation_play_maxtime)
        {
            emoticonBlock.GetComponent<Animator>().Play("Emoticon_Click_Disappear");
            if(emoticonBlock.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Emoticon_Click_Disappear"))
            {
                if(emoticonBlock.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
                {
                    emoticonBlock.SetActive(false);
                    is_emoticon_block_on = false;
                    time = 0; //이 부분에서 버그가 일어날수 있으니 유의
                }
            }
        }
    }
   
        




    //get set plus 함수

    public int Get_Emoticon_open_Count()
    {
        return emoticon_open_Count;
    }
    public GameObject get_Emoticon_Block()
    {
        return emoticonBlock;
    }

    public void Set_Time(int Time)
    {
        time = Time;
    }

    public void Plus_Emoticon_open_Count(int count)
    {
        emoticon_open_Count += count;
    }

}
